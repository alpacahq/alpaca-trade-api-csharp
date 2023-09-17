namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the single active stock information from Alpaca APIs.
/// </summary>
[CLSCompliant(false)]
public interface IActiveStock
{
    /// <summary>
    /// Gets the stock instrument symbol name.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the current instrument volume value.
    /// </summary>
    [UsedImplicitly]
    Decimal Volume { get; }

    /// <summary>
    /// Gets the current instrument trade count value.
    /// </summary>
    [UsedImplicitly]
    UInt64 TradeCount { get; }
}
