using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class RateThrottler : IThrottler, IDisposable
    {
        public Int32 MaxRetryAttempts { get; set; }

        public HashSet<Int32> RetryHttpStatuses { get; set; }

        /// <summary>
        /// Used to create a random length delay when server responds with a Http status like 503, but provides no Retry-After header.
        /// </summary>
        private readonly Random _randomRetryWait;

        /// <summary>
        /// Times (in millisecond ticks) at which the semaphore should be exited.
        /// </summary>
        private readonly ConcurrentQueue<Int32> _exitTimes;

        /// <summary>
        /// Semaphore used to count and limit the number of occurrences per unit time.
        /// </summary>
        private readonly SemaphoreSlim _throttleSemaphore;

        /// <summary>
        /// Timer used to trigger exiting the semaphore.
        /// </summary>
        private readonly Timer _exitTimer;

        /// <summary>
        /// The length of the time unit, in milliseconds.
        /// </summary>
        private readonly Int32 _timeUnitMilliseconds;

        /// <summary>
        /// Semaphore used to exact thread control over all-stop
        /// </summary>
        private readonly SemaphoreSlim _allStopSemaphore;

        /// <summary>
        /// Flag that all-stop is in effect
        /// </summary>
        private bool _allStop = false;

        /// <summary>
        /// Ticks at which all stop is over
        /// </summary>
        private Int32 _allStopUntil = 0;

        /// <summary>
        /// Initializes a <see cref="RateThrottler" /> with a rate of <paramref name="occurrences" />
        /// per <paramref name="timeUnit" />.
        /// </summary>
        /// <param name="occurrences">Number of occurrences allowed per unit of time.</param>
        /// <param name="timeUnit">Length of the time unit.</param>
        /// <param name="maxRetryAttempts">Number of times to retry an Http request, if the status code is one of the <paramref name="retryHttpStatuses"/></param>
        /// <param name="retryHttpStatuses">Http status codes that trigger a retry, up to the <paramref name="maxRetryAttempts"/></param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="occurrences" />, <paramref name="maxRetryAttempts"/> or <paramref name="timeUnit" /> is negative.
        /// </exception>
        public RateThrottler(
            Int32 occurrences,
            TimeSpan timeUnit,
            Int32 maxRetryAttempts,
            HashSet<Int32> retryHttpStatuses = null)
        {
            if (occurrences <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(occurrences),
                    "Number of occurrences must be a positive integer");
            }
            if (maxRetryAttempts <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetryAttempts),
                    "Number of maximal retry attempts must be a positive integer");
            }
            if (timeUnit != timeUnit.Duration())
            {
                throw new ArgumentOutOfRangeException(nameof(timeUnit), "Time unit must be a positive span of time");
            }
            if (timeUnit >= TimeSpan.FromMilliseconds(UInt32.MaxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(timeUnit), "Time unit must be less than 2^32 milliseconds");
            }

            _timeUnitMilliseconds = (Int32) timeUnit.TotalMilliseconds;
            MaxRetryAttempts = maxRetryAttempts;
            RetryHttpStatuses = retryHttpStatuses ?? new HashSet<Int32>();

            // TODO: If server logic fixed to provide Retry-After, this will be dead code to remove
            _randomRetryWait = new Random();

            // Create the all stop semaphore, limited to 1 thread at a time
            _allStopSemaphore = new SemaphoreSlim(1);

            // Create the throttle semaphore, with the number of occurrences as the maximum count.
            _throttleSemaphore = new SemaphoreSlim(occurrences, occurrences);

            // Create a queue to hold the semaphore exit times.
            _exitTimes = new ConcurrentQueue<Int32>();

            // Create a timer to exit the semaphore. Use the time unit as the original
            // interval length because that's the earliest we will need to exit the semaphore.
            _exitTimer = new Timer(exitTimerCallback, null, _timeUnitMilliseconds, -1);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _throttleSemaphore.Dispose();
            _allStopSemaphore.Dispose();
            _exitTimer.Dispose();
        }

        /// <inheritdoc />
        public void WaitToProceed()
        {

            // Block until any all stops are over
            Int32 stopTicks;
            while (_allStop && (stopTicks = unchecked(_allStopUntil - Environment.TickCount)) > 0)
            {
                Task.Delay(stopTicks).Wait();
            }
            if (_allStop)
            {
                endAllStop();
            }

            // Block until we can enter the semaphore or until the timeout expires.
            var entered = _throttleSemaphore.Wait(Timeout.Infinite);

            // If we entered the semaphore, compute the corresponding exit time 
            // and add it to the queue.
            if (entered)
            {
                var timeToExit = unchecked(Environment.TickCount + _timeUnitMilliseconds);
                _exitTimes.Enqueue(timeToExit);
            }
        }

        // Callback for the exit timer that exits the semaphore based on exit times 
        // in the queue and then sets the timer for the nextexit time.
        private void exitTimerCallback(Object state)
        {
            // Block until any all stops are over
            Int32 timeUntilNextCheck;
            if (_allStop && (timeUntilNextCheck = unchecked(_allStopUntil - Environment.TickCount)) > 0)
            {
                _exitTimer.Change(timeUntilNextCheck, -1);
                return;
            }
            if (_allStop)
            {
                endAllStop();
            }

            // While there are exit times that are passed due still in the queue,
            // exit the semaphore and dequeue the exit time.
            Int32 exitTime;
            while (_exitTimes.TryPeek(out exitTime)
                   && unchecked(exitTime - Environment.TickCount) <= 0)
            {
                _throttleSemaphore.Release();
                _exitTimes.TryDequeue(out exitTime);
            }

            // Try to get the next exit time from the queue and compute
            // the time until the next check should take place. If the 
            // queue is empty, then no exit times will occur until at least
            // one time unit has passed.
            if (_exitTimes.TryPeek(out exitTime))
            {
                timeUntilNextCheck = unchecked(exitTime - Environment.TickCount);
            }
            else
            {
                timeUntilNextCheck = _timeUnitMilliseconds;
            }

            // Set the timer.
            _exitTimer.Change(timeUntilNextCheck, -1);
        }

        /// <inheritdoc />
        public void AllStop(Int32 milliseconds)
        {
            _allStopSemaphore.Wait();
            try
            {
                _allStopUntil = unchecked(Environment.TickCount + milliseconds);
                _allStop = true;
            }
            finally { _allStopSemaphore.Release(); }
        }

        /// <summary>
        /// Thread-safe removal of all stop flag when duration of all stop has concluded
        /// </summary>
        private void endAllStop()
        {
            _allStopSemaphore.Wait();
            try
            {
                // End if no other request has come in for a longer all stop
                if (_allStop && unchecked(_allStopUntil - Environment.TickCount) <= 0)
                    _allStop = false;
            }
            finally { _allStopSemaphore.Release(); }
        }

        /// <inheritdoc />
        public bool CheckHttpResponse(HttpResponseMessage response)
        {
            // Adhere to server reported instructions
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            // Accomodate server specified delays in Retry-After headers
            var retryAfter = response.Headers.RetryAfter?.Delta?.TotalMilliseconds ?? (response.Headers.RetryAfter?.Date?.LocalDateTime - DateTime.Now)?.TotalMilliseconds;
            if (retryAfter != null)
            {
                AllStop((Int32)retryAfter);
                return false;
            }

            // Server unavailable, or Too many requests (429 can happen when this client competes with another client, e.g. mobile app)
            if (response.StatusCode == (HttpStatusCode)429 || response.StatusCode == (HttpStatusCode)503)
            {
                // TODO: If server logic fixed to provide Retry-After, this whole IF block will be dead code to remove
                AllStop(_randomRetryWait.Next(1000, 5000));
                return false;
            }

            // Accomodate retries on statuses indicated by caller
            if (RetryHttpStatuses.Contains((int)response.StatusCode))
            {
                return false;
            }

            // Allow framework to throw the exception
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}