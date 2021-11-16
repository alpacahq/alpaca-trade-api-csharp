namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for all REST API client instances.
/// </summary>
public abstract class AlpacaClientConfigurationBase
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaClientConfigurationBase"/> class.
    /// </summary>
    protected AlpacaClientConfigurationBase(
        Uri apiEndpoint)
    {
        SecurityId = new SecretKey(String.Empty, String.Empty);
        ThrottleParameters = ThrottleParameters.Default;
        ApiEndpoint = apiEndpoint;
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
    public ThrottleParameters ThrottleParameters { get; set; }

    /// <summary>
    /// Gets or sets <see cref="HttpClient"/> instance for connecting.
    /// </summary>
    [UsedImplicitly]
    public HttpClient? HttpClient { get; set; }

    // ReSharper disable once MemberCanBeProtected.Global
    internal abstract Uri GetApiEndpoint();

    internal HttpClient GetConfiguredHttpClient() =>
        ensureIsValidAndGetHttpClient().Configure(SecurityId, GetApiEndpoint());

    private HttpClient ensureIsValidAndGetHttpClient()
    {
        if (SecurityId is null)
        {
            throw new InvalidOperationException(
                $"The value of '{nameof(SecurityId)}' property shouldn't be null.");
        }

        if (ApiEndpoint is null)
        {
            throw new InvalidOperationException(
                $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
        }

        if (ThrottleParameters is null)
        {
            throw new InvalidOperationException(
                $"The value of '{nameof(ThrottleParameters)}' property shouldn't be null.");
        }

        return HttpClient ?? ThrottleParameters.GetHttpClient();
    }
}
