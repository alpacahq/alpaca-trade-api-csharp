namespace Alpaca.Markets;

/// <summary>
/// Encapsulates basic bar information for Alpaca APIs.
/// </summary>
[CLSCompliant(false)]
public interface IBar
{
    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the beginning time of this bar in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimeUtc { get; }

    /// <summary>
    /// Gets bar open price.
    /// </summary>
    [UsedImplicitly]
    Decimal Open { get; }

    /// <summary>
    /// Gets bar high price.
    /// </summary>
    [UsedImplicitly]
    Decimal High { get; }

    /// <summary>
    /// Gets bar low price.
    /// </summary>
    [UsedImplicitly]
    Decimal Low { get; }

    /// <summary>
    /// Gets bar close price.
    /// </summary>
    [UsedImplicitly]
    Decimal Close { get; }

    /// <summary>
    /// Gets bar trading volume.
    /// </summary>
    [UsedImplicitly]
    Decimal Volume { get; }

    /// <summary>
    /// Gets bar volume weighted average price.
    /// </summary>
    [UsedImplicitly]
    Decimal Vwap { get; }

    /// <summary>
    /// Gets total trades count for this bar.
    /// </summary>
    [UsedImplicitly]
    UInt64 TradeCount { get; }
}
