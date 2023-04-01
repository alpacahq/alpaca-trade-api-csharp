namespace Alpaca.Markets;

/// <summary>
/// Encapsulates access to the latest available rate limit information for this client.
/// </summary>
public interface IRateLimitProvider
{
    /// <summary>
    /// Gets the latest available rate limit information or default values if no information is available.
    /// </summary>
    /// <returns>The latest rate limit data from the server or an empty object if no request was made.</returns>
    public IRateLimitValues GetRateLimitValues();
}
