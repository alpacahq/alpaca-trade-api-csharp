namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the order book price/size pair information.
/// </summary>
public interface IOrderBookEntry
{
    /// <summary>
    /// Gets price level.
    /// </summary>
    [UsedImplicitly]
    Decimal Price { get; }

    /// <summary>
    /// Gets quantity.
    /// </summary>
    [UsedImplicitly]
    Decimal Size { get; }
}
