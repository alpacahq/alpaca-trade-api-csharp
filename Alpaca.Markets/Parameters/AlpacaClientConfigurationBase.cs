namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for all REST API client instances.
/// </summary>
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public abstract class AlpacaClientConfigurationBase
{
    private static readonly Func<HttpMessageHandler, HttpMessageHandler> _defaultHttpMessageHandlerFactory = handler => handler;

    /// <summary>
    /// Creates new instance of <see cref="AlpacaClientConfigurationBase"/> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="apiEndpoint"/> argument is <c>null</c>.
    /// </exception>
    protected AlpacaClientConfigurationBase(
        Uri apiEndpoint)
    {
        SecurityId = new SecretKey(String.Empty, String.Empty);
        ThrottleParameters = ThrottleParameters.Default;
        ApiEndpoint = apiEndpoint.EnsureNotNull();
    }

    /// <summary>
    /// Security identifier for API authentication.
    /// </summary>
    public SecurityKey SecurityId { get; set; }

    /// <summary>
    /// Gets or sets Alpaca Data API base URL.
    /// </summary>
    public Uri ApiEndpoint { get; set; }

    /// <summary>
    /// Gets or sets REST API throttling parameters.
    /// </summary>
    [UsedImplicitly]
    public ThrottleParameters ThrottleParameters { get; [UsedImplicitly] set; }

    /// <summary>
    /// Gets or sets <see cref="HttpClient"/> instance for connecting.
    /// </summary>
    [UsedImplicitly]
    public HttpClient? HttpClient { get; set; }

    [UsedImplicitly]
    internal Func<HttpMessageHandler, HttpMessageHandler>? HttpMessageHandlerFactory { get; set; }

    // ReSharper disable once MemberCanBeProtected.Global
    internal abstract Uri GetApiEndpoint();

    internal HttpClient GetConfiguredHttpClient() =>
        ensureIsValidAndGetHttpClient().Configure(SecurityId, GetApiEndpoint());

    private HttpClient ensureIsValidAndGetHttpClient()
    {
        ThrottleParameters.EnsurePropertyNotNull();
        ApiEndpoint.EnsurePropertyNotNull();
        SecurityId.EnsurePropertyNotNull();

        return HttpClient ?? ThrottleParameters.GetHttpClient(
            HttpMessageHandlerFactory ?? _defaultHttpMessageHandlerFactory);
    }
}
