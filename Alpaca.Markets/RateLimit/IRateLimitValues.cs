namespace Alpaca.Markets;

/// <summary>
/// Provides information about client-specific rate limit values.
/// </summary>
public interface IRateLimitValues
{
    /// <summary>
    /// Gets the total request-per-minute limit for the current client.
    /// </summary>
    Int32 Limit { get; }

    /// <summary>
    /// Get the remaining number of requests allowed in the current time interval.
    /// </summary>
    Int32 Remaining { get; }

    /// <summary>
    /// Gets end of the current time interval for requests limiting.
    /// </summary>
    DateTime ResetTimeUtc { get; }
}
