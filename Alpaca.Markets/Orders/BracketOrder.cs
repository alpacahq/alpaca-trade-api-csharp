using System;
using JetBrains.Annotations;

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
            StopLoss = baseOrder.StopLoss(stopLossStopPrice, stopLossLimitPrice);
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
}
