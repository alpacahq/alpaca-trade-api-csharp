namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base data for ordinal order types, never used directly by any code.
/// </summary>
public abstract class SimpleOrderBase : OrderBase
{
    /// <summary>
    /// Creates new instance of the <see cref="SimpleOrderBase"/> class.
    /// </summary>
    /// <param name="symbol">Alpaca symbol for order.</param>
    /// <param name="quantity">Order quantity (absolute value).</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="type">Order type (market, limit, stop, stop-limit).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    protected internal SimpleOrderBase(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        OrderType type)
        : base(
            symbol,
            quantity,
            side,
            type)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="TakeProfitOrder"/> order from the current order.
    /// </summary>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <returns>New advanced order representing pair of original order and take profit order.</returns>
    public TakeProfitOrder TakeProfit(
        Decimal takeProfitLimitPrice) =>
        new(
            this,
            takeProfitLimitPrice);

    /// <summary>
    /// Creates a new instance of the <see cref="StopLossOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    public StopLossOrder StopLoss(
        Decimal stopLossStopPrice) =>
        new(
            this,
            stopLossStopPrice,
            null);

    /// <summary>
    /// Creates a new instance of the <see cref="StopLossOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    public StopLossOrder StopLoss(
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        new(
            this,
            stopLossStopPrice,
            stopLossLimitPrice);

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
    /// </summary>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <returns>New advanced order representing an original order plus pair of take profit and stop loss orders.</returns>
    [UsedImplicitly]
    public BracketOrder Bracket(
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice) =>
        new(
            this,
            takeProfitLimitPrice,
            stopLossStopPrice,
            null);

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
    /// </summary>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
    /// <returns>New advanced order representing an original order plus pair of take profit and stop loss orders.</returns>
    [UsedImplicitly]
    public BracketOrder Bracket(
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        new(
            this,
            takeProfitLimitPrice,
            stopLossStopPrice,
            stopLossLimitPrice);
}
