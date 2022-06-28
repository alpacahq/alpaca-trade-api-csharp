namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic order book information from Alpaca APIs.
/// </summary>
public interface IOrderBook
{
    /// <summary>
    /// Gets asset name.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets quote timestamp in UTC time zone.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets identifier of source exchange.
    /// </summary>
    [UsedImplicitly]
    String Exchange { get; }

    /// <summary>
    /// Gets bids price/quantity pairs list.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IOrderBookEntry> Bids { get; }

    /// <summary>
    /// Gets asks price/quantity pairs list.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IOrderBookEntry> Asks { get; }

    /// <summary>
    /// Gets the order book reset flag.
    /// </summary>
    [UsedImplicitly]
    Boolean IsReset { get; }
}
