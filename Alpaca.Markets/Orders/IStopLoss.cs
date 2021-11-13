namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the price information about the stop loss order.
/// </summary>
public interface IStopLoss
{
    /// <summary>
    /// Gets the stop loss stop price.
    /// </summary>
    Decimal StopPrice { get; }

    /// <summary>
    /// Gets the stop loss limit price.
    /// </summary>
    Decimal? LimitPrice { get; }
}
