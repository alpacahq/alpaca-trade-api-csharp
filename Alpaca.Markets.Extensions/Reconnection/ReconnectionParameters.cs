namespace Alpaca.Markets.Extensions;

/// <summary>
/// Automatic reconnection parameters for streaming client with auto-reconnection support.
/// </summary>
public sealed class ReconnectionParameters
{
    /// <summary>
    /// Gets or sets the maximum number of reconnection attempts in case of a connection closing.
    /// </summary>
    public Int32 MaxReconnectionAttempts { get; [UsedImplicitly] set; } = 5;

    /// <summary>
    /// Gets or sets the minimum delay between different reconnection attempts.
    /// </summary>
    public TimeSpan MinReconnectionDelay { get; [UsedImplicitly] set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the maximum delay between repeated reconnection attempts.
    /// </summary>
    public TimeSpan MaxReconnectionDelay { get; [UsedImplicitly] set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// Gets the default reconnection parameters - 5 attempts with delay from 1 to 5 seconds.
    /// </summary>
    public static ReconnectionParameters Default => new();
}
