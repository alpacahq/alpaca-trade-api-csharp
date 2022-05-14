namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the price information about the take profit order.
/// </summary>
public interface ITakeProfit
{
    /// <summary>
    /// Gets the profit taking limit price.
    /// </summary>
    Decimal LimitPrice { get; }
}
