namespace Alpaca.Markets;

/// <summary>
/// A market order is a request to buy or sell a security at the currently available market price.
/// </summary>
/// <remarks>See <a href="https://alpaca.markets/docs/trading/orders/#market-order">Alpaca Order Documentation</a> for more information.</remarks>
public sealed class MarketOrder : SimpleOrderBase
{
    internal MarketOrder(
        String symbol,
        OrderQuantity quantity,
        OrderSide side)
        : base(
            symbol, quantity, side,
            OrderType.Market)
    {
    }

    /// <summary>
    /// Creates new buy market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static MarketOrder Buy(
        String symbol,
        OrderQuantity quantity) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Buy);

    /// <summary>
    /// Creates new sell market order using specified symbol and quantity.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
    [UsedImplicitly]
    public static MarketOrder Sell(
        String symbol,
        OrderQuantity quantity) =>
        new(symbol.EnsureNotNull(), quantity, OrderSide.Sell);
}
