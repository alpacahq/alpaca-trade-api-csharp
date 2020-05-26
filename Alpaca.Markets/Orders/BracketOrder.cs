using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing bracket order on the Alpaca REST API.
    /// </summary>
    public sealed class BracketOrder : AdvancedOrderBase
    {
        internal BracketOrder(
            SimpleOrderBase order,
            Decimal takeProfitLimitPrice,
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice)
            : base(
                order, 
                OrderClass.Bracket)
        {
            TakeProfit = order.TakeProfit(takeProfitLimitPrice);
            StopLoss = order.StopLoss(stopLossStopPrice, stopLossLimitPrice);
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
