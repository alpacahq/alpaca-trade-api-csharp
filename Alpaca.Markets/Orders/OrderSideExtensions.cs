namespace Alpaca.Markets;

/// <summary>
/// Set of extensions methods for creating the <see cref="OrderBase"/> inheritors.
/// </summary>
public static class OrderSideExtensions
{
    /// <summary>
    /// Creates new market order using specified side, symbol, and quantity.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static MarketOrder Market(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity) =>
        new(symbol, quantity, orderSide);

    /// <summary>
    /// Creates new stop order using specified side, symbol, quantity, and stop price.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopOrder Stop(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice) =>
        new(symbol, quantity, orderSide, stopPrice);

    /// <summary>
    /// Creates new limit order using specified side, symbol, quantity, and limit price.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static LimitOrder Limit(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        Decimal limitPrice) =>
        new(symbol, quantity, orderSide, limitPrice);

    /// <summary>
    /// Creates new limit order using specified side, symbol, quantity, stop, and limit prices.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopLimitOrder StopLimit(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice,
        Decimal limitPrice) =>
        new(symbol, quantity, orderSide, stopPrice, limitPrice);

    /// <summary>
    /// Creates new trailing stop order using specified side, symbol, quantity, and trail offset.
    /// </summary>
    /// <param name="orderSide">Order side (buy or sell).</param>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="trailOffset">Order trail offset.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="TrailingStopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static TrailingStopOrder TrailingStop(
        this OrderSide orderSide,
        String symbol,
        OrderQuantity quantity,
        TrailOffset trailOffset) =>
        new(symbol, quantity, orderSide, trailOffset);
}
