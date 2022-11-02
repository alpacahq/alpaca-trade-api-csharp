namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the auction entry information from Alpaca APIs.
/// </summary>
public interface IAuctionEntry
{
    /// <summary>
    /// Gets auction timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets identifier of auction source exchange.
    /// </summary>
    [UsedImplicitly]
    String Exchange { get; }

    /// <summary>
    /// Gets auction price level.
    /// </summary>
    [UsedImplicitly]
    Decimal Price { get; }

    /// <summary>
    /// Gets auction quantity.
    /// </summary>
    [UsedImplicitly]
    Decimal Size { get; }

    /// <summary>
    /// Gets auction condition.
    /// </summary>
    [UsedImplicitly]
    String Condition { get; }
}
