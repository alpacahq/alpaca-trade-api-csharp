namespace Alpaca.Markets;

/// <summary>
/// A limit order is an order to buy or sell at a specified price or better.
/// </summary>
/// <remarks>See <a href="https://alpaca.markets/docs/trading/orders/#limit-order">Alpaca Order Documentation</a> for more information.</remarks>
public sealed class LimitOrder : SimpleOrderBase
{
    internal LimitOrder(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        Decimal limitPrice)
        : base(
            symbol, quantity, side,
            OrderType.Limit) =>
        LimitPrice = limitPrice;

    /// <summary>
    /// Gets or sets the new order limit price.
    /// </summary>
    public Decimal LimitPrice { get; }

    /// <summary>
    /// Creates new buy market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static LimitOrder Buy(
        String symbol,
        OrderQuantity quantity,
        Decimal limitPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Buy, limitPrice);

    /// <summary>
    /// Creates new sell market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static LimitOrder Sell(
        String symbol,
        OrderQuantity quantity,
        Decimal limitPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Sell, limitPrice);

    /// <summary>
    /// Creates a new instance of the <see cref="OneCancelsOtherOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    [UsedImplicitly]
    public OneCancelsOtherOrder OneCancelsOther(
        Decimal stopLossStopPrice) =>
        new(this, stopLossStopPrice, null);

    /// <summary>
    /// Creates a new instance of the <see cref="OneCancelsOtherOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    [UsedImplicitly]
    public OneCancelsOtherOrder OneCancelsOther(
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        new(this, stopLossStopPrice, stopLossLimitPrice);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithLimitPrice(LimitPrice);
}
