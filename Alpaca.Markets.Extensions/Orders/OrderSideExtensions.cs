namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extensions methods for creating the <see cref="OrderBase"/> inheritors.
/// </summary>
public static class OrderSideExtensions
{
    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order using
    /// specified side, symbol, quantity, take profit, and stop loss prices.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset name.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>
    /// New advanced order representing an original order plus pair of take profit and stop loss orders.
    /// </returns>
    [UsedImplicitly]
    public static BracketOrder Bracket(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice) =>
        orderSide.Market(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice);

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order using
    /// specified side, symbol, quantity, take profit, and stop loss prices.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset name.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>
    /// New advanced order representing an original order plus pair of take profit and stop loss orders.
    /// </returns>
    [UsedImplicitly]
    public static BracketOrder Bracket(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        orderSide.Market(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice, stopLossLimitPrice);
}
