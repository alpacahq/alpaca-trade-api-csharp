namespace Alpaca.Markets;

/// <summary>
/// A bracket order is a chain of three orders that can be used to manage your position entry and exit.
/// It is a common use case of an OTOCO (One Triggers OCO {One Cancels Other}) order.
/// <para>See https://alpaca.markets/docs/trading/orders/#bracket-orders</para>
/// </summary>
public sealed class BracketOrder : AdvancedOrderBase
{
    internal BracketOrder(
        SimpleOrderBase baseOrder,
        Decimal takeProfitLimitPrice,
        Decimal stopLossStopPrice,
        Decimal? stopLossLimitPrice)
        : base(
            baseOrder,
            OrderClass.Bracket)
    {
        TakeProfit = baseOrder.TakeProfit(takeProfitLimitPrice);
        StopLoss = stopLossLimitPrice.HasValue
            ? baseOrder.StopLoss(stopLossStopPrice, stopLossLimitPrice.Value)
            : baseOrder.StopLoss(stopLossStopPrice);
    }

    /// <summary>
    /// Gets prices for take profit order for the bracket order.
    /// </summary>
    [UsedImplicitly]
    public ITakeProfit TakeProfit { get; }

    /// <summary>
    /// Gets prices for stop loss order for the bracket order.
    /// </summary>
    [UsedImplicitly]
    public IStopLoss StopLoss { get; }

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithTakeProfit(TakeProfit)
            .WithStopLoss(StopLoss);
}
