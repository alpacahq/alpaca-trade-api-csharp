namespace Alpaca.Markets;

/// <summary>
/// A stop (market) order is an order to buy or sell a security when its price moves past a particular point,
/// ensuring a higher probability of achieving a predetermined entry or exit price.
/// <para>See https://alpaca.markets/docs/trading/orders/#stop-order</para>
/// </summary>
public sealed class StopOrder : SimpleOrderBase
{
    internal StopOrder(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        Decimal stopPrice
    )
        : base(
            symbol, quantity, side,
            OrderType.Stop) =>
        StopPrice = stopPrice;

    /// <summary>
    /// Gets or sets the new order stop price.
    /// </summary>
    [UsedImplicitly]
    public Decimal StopPrice { get; }

    /// <summary>
    /// Creates new buy stop order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopOrder Buy(
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Buy, stopPrice);

    /// <summary>
    /// Creates new sell buy order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="stopPrice">Order stop price.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static StopOrder Sell(
        String symbol,
        OrderQuantity quantity,
        Decimal stopPrice) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Sell, stopPrice);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithStopPrice(StopPrice);
}
