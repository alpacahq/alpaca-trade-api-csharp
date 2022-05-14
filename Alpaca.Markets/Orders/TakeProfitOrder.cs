namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data required for placing take profit order on the Alpaca REST API.
/// </summary>
public sealed class TakeProfitOrder : AdvancedOrderBase, ITakeProfit
{
    internal TakeProfitOrder(
        SimpleOrderBase baseOrder,
        Decimal limitPrice)
        : base(
            baseOrder,
            OrderClass.OneTriggersOther) =>
        LimitPrice = limitPrice;

    /// <inheritdoc />
    public Decimal LimitPrice { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    [UsedImplicitly]
    public BracketOrder StopLoss(
        Decimal stopLossStopPrice) =>
        new(BaseOrder, LimitPrice, stopLossStopPrice, null);

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
    /// </summary>
    /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
    /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
    /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
    [UsedImplicitly]
    public BracketOrder StopLoss(
        Decimal stopLossStopPrice,
        Decimal stopLossLimitPrice) =>
        new(BaseOrder, LimitPrice, stopLossStopPrice, stopLossLimitPrice);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithTakeProfit(this);
}
