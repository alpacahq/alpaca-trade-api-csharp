using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Polly;

namespace Alpaca.Markets
{
    /// <summary>
    /// Helper class for storing parameters required for initializing rate throttler in <see cref="AlpacaTradingClient"/> class.
    /// </summary>
    public sealed class ThrottleParameters
    {
        private sealed class CustomHttpHandler : HttpClientHandler
        {
            private readonly IAsyncPolicy<HttpResponseMessage> _asyncPolicy;

            public CustomHttpHandler(
                IAsyncPolicy<HttpResponseMessage> asyncPolicy) =>
                _asyncPolicy = asyncPolicy;

            /// <inheritdoc />
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken) =>
                await _asyncPolicy.ExecuteAsync(() => base.SendAsync(request, cancellationToken))
                    .ConfigureAwait(false);
        }

        private const UInt32 DefaultMaxRetryAttempts = 5;

        private static readonly HttpStatusCode[] _defaultHttpStatuses =
        {
#if NETSTANDARD2_0 || NETFRAMEWORK
            (HttpStatusCode) 429,
#else
            HttpStatusCode.TooManyRequests,
#endif
            HttpStatusCode.BadGateway,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.ServiceUnavailable
        };

        private static readonly SocketError[] _defaultSocketErrorCodes =
        {
            SocketError.TimedOut,
            SocketError.HostNotFound, 
            SocketError.TryAgain
        };

        private readonly HashSet<HttpStatusCode> _retryHttpStatuses;

        private readonly HashSet<SocketError> _retrySocketErrorCodes;

        private readonly Random _random = new ();

        private ThrottleParameters()
            : this(
                DefaultMaxRetryAttempts,
                _defaultSocketErrorCodes,
                _defaultHttpStatuses)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="ThrottleParameters"/> object.
        /// </summary>
        /// <param name="maxRetryAttempts"></param>
        /// <param name="retrySocketErrorCodes"></param>
        /// <param name="retryHttpStatuses"></param>
        [CLSCompliant(false)]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public ThrottleParameters(
            UInt32 maxRetryAttempts,
            IEnumerable<SocketError> retrySocketErrorCodes,
            IEnumerable<HttpStatusCode> retryHttpStatuses)
        {
            MaxRetryAttempts = maxRetryAttempts;
            _retrySocketErrorCodes = new HashSet<SocketError>(
                // ReSharper disable once ConstantNullCoalescingCondition
                retrySocketErrorCodes ?? _defaultSocketErrorCodes);
            _retryHttpStatuses = new HashSet<HttpStatusCode>(
                // ReSharper disable once ConstantNullCoalescingCondition
                retryHttpStatuses ?? _defaultHttpStatuses);
        }

        /// <summary>
        /// Gets throttle parameters initialized with default values.
        /// </summary>
        public static ThrottleParameters Default { get; } = new ();

        /// <summary>
        /// Gets or sets maximal number of retry attempts for single request.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32 MaxRetryAttempts { get; [UsedImplicitly] set; }

        /// <summary>
        /// Gets set of HTTP status codes which when received should initiate a retry of the affected request
        /// </summary>
        public ISet<HttpStatusCode> RetryHttpStatuses => _retryHttpStatuses;

        /// <summary>
        /// Gets set of socket error codes which when received should initiate a retry of the affected request
        /// </summary>
        public ISet<SocketError> RetrySocketErrorCodes => _retrySocketErrorCodes;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [CLSCompliant(false)]
        public IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy()
        {
            var socketErrorsPolicy = Policy
                .HandleInner<SocketException>(
                    exception =>
                        RetrySocketErrorCodes.Contains((SocketError)exception.ErrorCode))
                .WaitAndRetryAsync(
                    (Int32) MaxRetryAttempts, getRandomDelay);

            var httpResponsesPolicy = Policy<HttpResponseMessage>
                .HandleResult(response =>
                    RetryHttpStatuses.Contains(response.StatusCode) ||
                    response.Headers.RetryAfter != null)
                .WaitAndRetryAsync(
                    (Int32) MaxRetryAttempts, getDelayFromHeader,
                    (_, _, _, _) => Task.CompletedTask); // Only for compiler satisfaction

            return socketErrorsPolicy.WrapAsync(httpResponsesPolicy);
        }

        internal HttpClient GetHttpClient() => 
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CA5399 // HttpClient is created without enabling CheckCertificateRevocationList
            new (new CustomHttpHandler(GetAsyncPolicy()), true);
#pragma warning restore CA5399 //  HttpClient is created without enabling CheckCertificateRevocationList
#pragma warning restore CA2000 // Dispose objects before losing scope

        private TimeSpan getDelayFromHeader(
            Int32 retryCount,
            DelegateResult<HttpResponseMessage> response,
            Context _) =>
            getDelayFromHeader(response.Result?.Headers.RetryAfter, retryCount);

        private TimeSpan getDelayFromHeader(
            RetryConditionHeaderValue? retryAfterHeader,
            Int32 retryCount) =>
            retryAfterHeader switch
            {
                {Date: not null} => retryAfterHeader.Date.Value.UtcDateTime.Subtract(DateTime.UtcNow),
                {Delta: not null} => retryAfterHeader.Delta.Value,
                _ => TimeSpan.Zero
            } + getRandomDelay(retryCount);

        private TimeSpan getRandomDelay(
            Int32 retryCount) =>
#pragma warning disable CA5394 // Do not use insecure randomness
            TimeSpan.FromMilliseconds(_random.Next(200, 400 * retryCount));
#pragma warning restore CA5394 // Do not use insecure randomness
    }
}
