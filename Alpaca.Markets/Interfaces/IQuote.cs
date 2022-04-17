namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic quote information from Alpaca APIs.
/// </summary>
public interface IQuote
{
    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets quote timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets identifier of bid source exchange.
    /// </summary>
    [UsedImplicitly]
    String BidExchange { get; }

    /// <summary>
    /// Gets identifier of ask source exchange.
    /// </summary>
    [UsedImplicitly]
    String AskExchange { get; }

    /// <summary>
    /// Gets bid price level (highest buy offer).
    /// </summary>
    [UsedImplicitly]
    Decimal BidPrice { get; }

    /// <summary>
    /// Gets ask price level (lowest sell offer).
    /// </summary>
    [UsedImplicitly]
    Decimal AskPrice { get; }

    /// <summary>
    /// Gets bid quantity.
    /// </summary>
    [UsedImplicitly]
    Decimal BidSize { get; }

    /// <summary>
    /// Gets ask quantity.
    /// </summary>
    [UsedImplicitly]
    Decimal AskSize { get; }

    /// <summary>
    /// Gets tape where trade occurred.
    /// </summary>
    [UsedImplicitly]
    String Tape { get; }

    /// <summary>
    /// Gets trade conditions list.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<String> Conditions { get; }
}
