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
    [UsedImplicitly]
    [SuppressMessage("Design", "CA1024:Use properties where appropriate",
        Justification = "Implementation returns the new object each time so it's better to not use property here.")]
    public IRateLimitValues GetRateLimitValues();
}
