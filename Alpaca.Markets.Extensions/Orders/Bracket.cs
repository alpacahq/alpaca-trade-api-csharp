namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of factory methods for creating the <see cref="BracketOrder"/> instances.
/// </summary>
public static class Bracket
{
    /// <summary>
    /// Creates a new instance of the buy <see cref="BracketOrder"/> order
    /// using specified symbol, quantity, take profit, and stop loss prices.
    /// </summary>
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
    public static BracketOrder Buy(
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice) =>
        MarketOrder.Buy(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice);

    /// <summary>
    /// Creates a new instance of the buy <see cref="BracketOrder"/> order
    /// using specified symbol, quantity, take profit, and stop loss prices.
    /// </summary>
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
    public static BracketOrder Buy(
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        MarketOrder.Buy(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice, stopLossLimitPrice);

    /// <summary>
    /// Creates a new instance of the sell <see cref="BracketOrder"/> order
    /// using specified symbol, quantity, take profit, and stop loss prices.
    /// </summary>
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
    public static BracketOrder Sell(
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice) =>
        MarketOrder.Sell(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice);

    /// <summary>
    /// Creates a new instance of the sell <see cref="BracketOrder"/> order
    /// using specified symbol, quantity, take profit, and stop loss prices.
    /// </summary>
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
    public static BracketOrder Sell(
        String symbol,
        OrderQuantity quantity,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        MarketOrder.Sell(symbol.EnsureNotNull(), quantity)
            .Bracket(takeProfitLimitPrice, stopLossStopPrice, stopLossLimitPrice);
}
