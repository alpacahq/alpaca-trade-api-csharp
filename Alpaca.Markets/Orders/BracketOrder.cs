using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing bracket order on the Alpaca REST API.
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
        public ITakeProfit TakeProfit { get; }
        
        /// <summary>
        /// Gets prices for stop loss order for the bracket order.
        /// </summary>
        public IStopLoss StopLoss { get; }

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithTakeProfit(TakeProfit)
                .WithStopLoss(StopLoss);
    }
}
