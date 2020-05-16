using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class OneCancelsOtherOrder : AdvancedOrderBase
    {
        internal OneCancelsOtherOrder(
            LimitOrder order,
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice)
            : base(
                order, 
                OrderClass.OneCancelsOther)
        {
            TakeProfit = order.TakeProfit(order.LimitPrice);
            StopLoss = order.StopLoss(stopLossStopPrice, stopLossLimitPrice);
        }

        /// <summary>
        /// Gets prices for take profit order for the advanced order types.
        /// </summary>
        public ITakeProfit TakeProfit { get; }
        
        /// <summary>
        /// Gets prices for stop loss order for the advanced order types.
        /// </summary>
        public IStopLoss StopLoss { get; }

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithTakeProfit(TakeProfit)
                .WithStopLoss(StopLoss);
    }
}
