using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class RateThrottler : IThrottler, IDisposable
    {
        private sealed class NextRetryGuard : IDisposable
        {
            private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

            /// <summary>
            /// Used to create a random length delay when server responds with a Http status like 503, but provides no Retry-After header.
            /// </summary>
            private readonly Random _randomRetryWait = new Random();

            private DateTime _nextRetryTime = DateTime.MinValue;

            public async Task WaitToProceed()
            {
                var delay = GetDelayTillNextRetryTime();

                if (delay.TotalMilliseconds < 0)
                {
                    return;
                }

                await Task.Delay(delay);
            }

            public void SetNextRetryTimeRandom()
            {
                // TODO: If server logic fixed to provide Retry-After, this whole IF block will be dead code to remove
                SetNextRetryTime(DateTime.UtcNow.AddMilliseconds(
                    _randomRetryWait.Next(1000, 5000)));
            }

            public void SetNextRetryTime(DateTime nextRetryTime)
            {
                if (nextRetryTime < DateTime.UtcNow)
                {
                    return;
                }

                _lock.EnterWriteLock();
                try
                {
                    if (nextRetryTime > _nextRetryTime)
                    {
                        _nextRetryTime = nextRetryTime;
                    }
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }

            public TimeSpan GetDelayTillNextRetryTime()
            {
                _lock.EnterReadLock();
                try
                {
                    return _nextRetryTime.Subtract(DateTime.UtcNow);
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            public void Dispose()
            {
                _lock?.Dispose();
            }
        }

        private readonly NextRetryGuard _nextRetryGuard = new NextRetryGuard();

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
            if (timeUnit >= TimeSpan.FromMilliseconds(uint.MaxValue))
            {
                throw new ArgumentOutOfRangeException(nameof(timeUnit), "Time unit must be less than 2^32 milliseconds");
            }

            _timeUnitMilliseconds = (Int32) timeUnit.TotalMilliseconds;
            MaxRetryAttempts = maxRetryAttempts;
            RetryHttpStatuses = retryHttpStatuses ?? new HashSet<Int32>();

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
            _nextRetryGuard.Dispose();
            _exitTimer.Dispose();
        }

        /// <inheritdoc />
        public Int32 MaxRetryAttempts { get; set; }

        /// <inheritdoc />
        public HashSet<Int32> RetryHttpStatuses { get; set; }

        /// <inheritdoc />
        public async Task WaitToProceed()
        {
            await _nextRetryGuard.WaitToProceed();

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
        // in the queue and then sets the timer for the next exit time.
        private void exitTimerCallback(Object state)
        {
            var nextRetryDelay = _nextRetryGuard.GetDelayTillNextRetryTime().TotalMilliseconds;
            if (nextRetryDelay > 0)
            {
                _exitTimer.Change((Int32)nextRetryDelay, Timeout.Infinite);
                return;
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
            var timeUntilNextCheck = _exitTimes.TryPeek(out exitTime)
                ? unchecked(exitTime - Environment.TickCount)
                :_timeUnitMilliseconds;

            // Set the timer.
            _exitTimer.Change(timeUntilNextCheck, Timeout.Infinite);
        }

        /// <inheritdoc />
        public Boolean CheckHttpResponse(HttpResponseMessage response)
        {
            // Adhere to server reported instructions
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            // Accomodate server specified delays in Retry-After headers
            var retryAfterHeader = response.Headers.RetryAfter;
            if (retryAfterHeader != null)
            {
                if (retryAfterHeader.Delta.HasValue)
                {
                    _nextRetryGuard.SetNextRetryTime(DateTime.UtcNow.Add(retryAfterHeader.Delta.Value));
                    return false;
                }

                if (retryAfterHeader.Date.HasValue)
                {
                    _nextRetryGuard.SetNextRetryTime(retryAfterHeader.Date.Value.UtcDateTime);
                }
            }

            // Server unavailable, or Too many requests (429 can happen when this client competes with another client, e.g. mobile app)
            if (response.StatusCode == (HttpStatusCode)429 ||
                response.StatusCode == (HttpStatusCode)503)
            {
                _nextRetryGuard.SetNextRetryTimeRandom();
                return false;
            }

            // Accomodate retries on statuses indicated by caller
            if (RetryHttpStatuses.Contains((Int32)response.StatusCode))
            {
                return false;
            }

            // Allow framework to throw the exception
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}