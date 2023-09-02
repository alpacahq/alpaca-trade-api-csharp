namespace Alpaca.Markets;

/// <summary>
/// Provides information about client-specific rate limit values.
/// </summary>
public interface IRateLimitValues
{
    /// <summary>
    /// Gets the total request-per-minute limit for the current client.
    /// </summary>
    [UsedImplicitly]
    Int32 Limit { get; }

    /// <summary>
    /// Get the remaining number of requests allowed in the current time interval.
    /// </summary>
    [UsedImplicitly]
    Int32 Remaining { get; }

    /// <summary>
    /// Gets end of the current time interval for requests limiting.
    /// </summary>
    [UsedImplicitly]
    DateTime ResetTimeUtc { get; }
}
