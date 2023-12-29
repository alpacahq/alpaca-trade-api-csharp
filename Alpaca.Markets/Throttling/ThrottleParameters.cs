using System.Net;
using System.Net.Http.Headers;
using Polly;

namespace Alpaca.Markets;

/// <summary>
/// Helper class for storing parameters required for initializing rate throttler in <see cref="IAlpacaTradingClient"/> class.
/// </summary>
public sealed class ThrottleParameters
{
    private sealed class CustomHttpHandler
#if NETFRAMEWORK
        : WinHttpHandler
#else
        : HttpClientHandler
#endif
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _asyncPolicy;

        private readonly TimeSpan _timeout;

        public CustomHttpHandler(
            IAsyncPolicy<HttpResponseMessage> asyncPolicy,
            TimeSpan timeout)
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            _asyncPolicy = asyncPolicy;
            _timeout = timeout;
        }

        /// <inheritdoc />
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            await _asyncPolicy
                .ExecuteAsync(() => sendAsyncWithTimeout(request, cancellationToken))
                .ConfigureAwait(false);

        private async Task<HttpResponseMessage> sendAsyncWithTimeout(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using var cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            cancellationTokenSource.CancelAfter(getTimeout(request));

            try
            {
                return await base.SendAsync(request, cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException exception)
                when (!cancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException("HTTP response timed out.", exception);
            }
        }

        private TimeSpan getTimeout(
            HttpRequestMessage request) =>
#if NET6_0_OR_GREATER
            request.Options.TryGetValue(RequestTimeoutOptionKey,
                out var requestSpecificTimeoutValue)
#else
            request.Properties.TryGetValue(
                RequestTimeoutOptionKey, out var value) &&
            value is TimeSpan requestSpecificTimeoutValue
#endif
                ? requestSpecificTimeoutValue
                : _timeout;
    }

    private static readonly TimeSpan _defaultHttpClientTimeout = new HttpClient().Timeout;

    private const Int32 DelayMinValueInMilliseconds = 200;

    private const Int32 DelayStepInMilliseconds = 400;

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
    [
        SocketError.TryAgain,
        SocketError.TimedOut,
        SocketError.WouldBlock,
        SocketError.NotConnected,
        SocketError.HostNotFound
    ];
   
    private readonly HashSet<HttpStatusCode> _retryHttpStatuses;

    private readonly HashSet<SocketError> _retrySocketErrorCodes;

    private readonly Random _random = new();

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
    [UsedImplicitly]
    [CLSCompliant(false)]
    public ThrottleParameters(
        UInt32 maxRetryAttempts,
        IEnumerable<SocketError> retrySocketErrorCodes,
        IEnumerable<HttpStatusCode> retryHttpStatuses)
    {
        MaxRetryAttempts = maxRetryAttempts;
        _retrySocketErrorCodes = new HashSet<SocketError>(
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            retrySocketErrorCodes ?? _defaultSocketErrorCodes);
        _retryHttpStatuses = new HashSet<HttpStatusCode>(
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            retryHttpStatuses ?? _defaultHttpStatuses);
    }

    /// <summary>
    /// Gets throttle parameters initialized with default values.
    /// </summary>
    public static ThrottleParameters Default { get; } = new();

#if NET6_0_OR_GREATER
    internal static readonly HttpRequestOptionsKey<TimeSpan> RequestTimeoutOptionKey =
        new(nameof(RequestTimeoutOptionKey));
#else
    internal const String RequestTimeoutOptionKey = nameof(RequestTimeoutOptionKey);
#endif

    /// <summary>
    /// Gets or sets maximum number of retry attempts for a single request.
    /// </summary>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public UInt32 MaxRetryAttempts { get; [UsedImplicitly] set; }

    /// <summary>
    /// Gets set of HTTP status codes which when received should initiate a retry of the affected request.
    /// </summary>
    [UsedImplicitly]
    public ISet<HttpStatusCode> RetryHttpStatuses => _retryHttpStatuses;

    /// <summary>
    /// Gets set of socket error codes which when received should initiate a retry of the affected request.
    /// </summary>
    [UsedImplicitly]
    public ISet<SocketError> RetrySocketErrorCodes => _retrySocketErrorCodes;

    /// <summary>
    /// Gets or sets the HTTP request timeout. Default <see cref="HttpClient"/> timeout value (100 sec)
    /// will be used if this property is equal to <c>null</c> or never set.
    /// </summary>
    [UsedImplicitly]
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Gets the custom message handler that supports reconnection logic configured with the current settings.
    /// </summary>
    [UsedImplicitly]
    public HttpMessageHandler GetMessageHandler() => new CustomHttpHandler(
        GetAsyncPolicy(), Timeout ?? _defaultHttpClientTimeout);

    /// <summary>
    /// Gets the custom Polly asynchronous execution policy (can be used by unit tests and DI containers).
    /// </summary>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public IAsyncPolicy<HttpResponseMessage> GetAsyncPolicy()
    {
        var socketErrorsPolicy = Policy
            .HandleInner<SocketException>(
                exception =>
                    RetrySocketErrorCodes.Contains(exception.SocketErrorCode))
            .WaitAndRetryAsync(
                (Int32)MaxRetryAttempts, getRandomDelay);

        var httpResponsesPolicy = Policy<HttpResponseMessage>
            .HandleResult(response =>
                RetryHttpStatuses.Contains(response.StatusCode) ||
                response.Headers.RetryAfter is not null)
            .WaitAndRetryAsync(
                (Int32)MaxRetryAttempts, getDelayFromHeader,
                (_, _, _, _) => Task.CompletedTask); // Only for compiler satisfaction

        return socketErrorsPolicy.WrapAsync(httpResponsesPolicy);
    }

    internal HttpClient GetHttpClient(
        Func<HttpMessageHandler, HttpMessageHandler> httpMessageHandlerFactory) =>
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CA5399 // HttpClient is created without enabling CheckCertificateRevocationList
        new(httpMessageHandlerFactory(GetMessageHandler()), true)
#pragma warning restore CA5399 //  HttpClient is created without enabling CheckCertificateRevocationList
#pragma warning restore CA2000 // Dispose objects before losing scope
        {
            Timeout = System.Threading.Timeout.InfiniteTimeSpan
        };

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
            { Date: not null } => retryAfterHeader.Date.Value.UtcDateTime.Subtract(DateTime.UtcNow),
            { Delta: not null } => retryAfterHeader.Delta.Value,
            _ => TimeSpan.Zero
        } + getRandomDelay(retryCount);

    private TimeSpan getRandomDelay(
        Int32 retryCount) =>
#pragma warning disable CA5394 // Do not use insecure randomness
            TimeSpan.FromMilliseconds(_random.Next(
                DelayMinValueInMilliseconds, DelayStepInMilliseconds * retryCount));
#pragma warning restore CA5394 // Do not use insecure randomness
}
