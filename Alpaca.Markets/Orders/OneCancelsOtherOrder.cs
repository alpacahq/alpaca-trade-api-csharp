using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing OCO order on the Alpaca REST API.
    /// </summary>
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
        public ITakeProfit TakeProfit { get; }
        
        /// <summary>
        /// Gets prices for stop loss order for the OCO order.
        /// </summary>
        public IStopLoss StopLoss { get; }

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithTakeProfit(TakeProfit)
                .WithStopLoss(StopLoss)
                .WithoutLimitPrice();
    }
}
