namespace Alpaca.Markets;

/// <summary>
/// A stop-limit order is a conditional trade over a set time frame that combines the features of a stop order with
/// those of a limit order and is used to mitigate risk.
/// The stop-limit order will be executed at a specified limit price, or better, after a given stop price has been reached.
/// <para>See https://alpaca.markets/docs/trading/orders/#stop-limit-order</para>
/// </summary>
public sealed class StopLimitOrder : SimpleOrderBase
{
    internal StopLimitOrder(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        Decimal stopPrice,
        Decimal limitPrice)
        : base(
            symbol, quantity, side,
            OrderType.StopLimit)
    {
        StopPrice = stopPrice;
        LimitPrice = limitPrice;
    }

    /// <summary>
    /// Gets or sets the new order stop price.
    /// </summary>
    [UsedImplicitly]
    public Decimal StopPrice { get; }

    /// <summary>
    /// Gets or sets the new order limit price.
    /// </summary>
    [UsedImplicitly]
    public Decimal LimitPrice { get; set; }

    /// <summary>
    /// Creates new buy stop limit order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopLimitOrder Buy(
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice,
        Decimal limitPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Buy, stopPrice, limitPrice);

    /// <summary>
    /// Creates new sell stop limit order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopLimitOrder Sell(
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice,
        Decimal limitPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Sell, stopPrice, limitPrice);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithLimitPrice(LimitPrice)
            .WithStopPrice(StopPrice);
}
