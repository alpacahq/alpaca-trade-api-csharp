namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data required for placing stop loss order on the Alpaca REST API.
/// </summary>
public sealed class StopLossOrder : AdvancedOrderBase, IStopLoss
{
    internal StopLossOrder(
        SimpleOrderBase baseOrder,
        Decimal stopPrice,
        Decimal? limitPrice)
        : base(
            baseOrder,
            OrderClass.OneTriggersOther)
    {
        LimitPrice = limitPrice;
        StopPrice = stopPrice;
    }

    /// <inheritdoc />
    public Decimal StopPrice { get; }

    /// <inheritdoc />
    public Decimal? LimitPrice { get; }

    /// <summary>
    /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
    /// </summary>
    /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
    /// <returns>New advanced order representing pair of original order and take profit order.</returns>
    [UsedImplicitly]
    public BracketOrder TakeProfit(
        Decimal takeProfitLimitPrice) =>
        new(BaseOrder, takeProfitLimitPrice, StopPrice, LimitPrice);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithStopLoss(this);
}
