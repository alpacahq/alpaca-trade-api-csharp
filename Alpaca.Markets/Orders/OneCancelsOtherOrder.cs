namespace Alpaca.Markets;

/// <summary>
/// OCO (One-Cancels-Other) is another type of advanced order type.
/// This is a set of two orders with the same side (buy/buy or sell/sell) and currently only exit order is supported.
/// In other words, this is the second part of the bracket orders where the entry order is already filled,
/// and you can submit the take-profit and stop-loss in one order submission.
/// </summary>
/// <remarks>See <a href="https://alpaca.markets/docs/trading/orders/#oco-orders">Alpaca Order Documentation</a> for more information.</remarks>
public sealed class OneCancelsOtherOrder : AdvancedOrderBase
{
    internal OneCancelsOtherOrder(
        LimitOrder limitOrder,
        Decimal stopLossStopPrice,
        Decimal? stopLossLimitPrice)
        : base(
            limitOrder,
            OrderClass.OneCancelsOther)
    {
        TakeProfit = limitOrder.TakeProfit(limitOrder.LimitPrice);
        StopLoss = stopLossLimitPrice.HasValue
            ? limitOrder.StopLoss(stopLossStopPrice, stopLossLimitPrice.Value)
            : limitOrder.StopLoss(stopLossStopPrice);
    }

    /// <summary>
    /// Gets prices for take profit order for the OCO order.
    /// </summary>
    [UsedImplicitly]
    public ITakeProfit TakeProfit { get; }

    /// <summary>
    /// Gets prices for stop loss order for the OCO order.
    /// </summary>
    [UsedImplicitly]
    public IStopLoss StopLoss { get; }

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithTakeProfit(TakeProfit)
            .WithStopLoss(StopLoss)
            .WithoutLimitPrice();
}
