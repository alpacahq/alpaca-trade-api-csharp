namespace Alpaca.Markets;

/// <summary>
/// Encapsulates current trading date information from Alpaca REST API.
/// </summary>
public interface IClock
{
    /// <summary>
    /// Gets current timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Returns <c>true</c> if trading day is open now.
    /// </summary>
    [UsedImplicitly]
    Boolean IsOpen { get; }

    /// <summary>
    /// Gets nearest trading day open time in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime NextOpenUtc { get; }

    /// <summary>
    /// Gets nearest trading day close time in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime NextCloseUtc { get; }
}
