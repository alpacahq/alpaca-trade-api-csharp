namespace Alpaca.Markets;

/// <summary>
/// Trailing stop orders allow you to continuously and automatically keep updating the stop price threshold based on the stock price movement.
/// <para>See https://alpaca.markets/docs/trading/orders/#trailing-stop-orders</para>
/// </summary>
public sealed class TrailingStopOrder : SimpleOrderBase
{
    internal TrailingStopOrder(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        TrailOffset trailOffset)
        : base(
            symbol, quantity, side,
            OrderType.TrailingStop) =>
        TrailOffset = trailOffset;

    /// <summary>
    /// Gets order trail offset value (in dollars or percent).
    /// </summary>
    [UsedImplicitly]
    public TrailOffset TrailOffset { get; }

    /// <summary>
    /// Creates new buy market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="trailOffset">Trailing stop order offset.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="TrailingStopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static TrailingStopOrder Buy(
        String symbol,
        OrderQuantity quantity,
        TrailOffset trailOffset) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Buy, trailOffset);

    /// <summary>
    /// Creates new sell market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="trailOffset">Trailing stop order offset.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="TrailingStopOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static TrailingStopOrder Sell(
        String symbol,
        OrderQuantity quantity,
        TrailOffset trailOffset) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Sell, trailOffset);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithTrailOffset(TrailOffset);
}
