using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SimpleOrderBase : OrderBase
    {
        internal SimpleOrderBase(
            String symbol,
            Int64 quantity,
            OrderSide orderSide,
            OrderType orderType)
            : base(
                symbol, 
                quantity, 
                orderSide, 
                orderType)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="takeProfitLimitPrice"></param>
        /// <returns></returns>
        public TakeProfitOrder TakeProfit(
            Decimal takeProfitLimitPrice) =>
            new TakeProfitOrder(
                this, 
                takeProfitLimitPrice);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StopLossOrder StopLoss(
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice = null) =>
            new StopLossOrder(
                this, 
                stopLossStopPrice,
                stopLossLimitPrice);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public BracketOrder Bracket(
            Decimal takeProfitLimitPrice,
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice = null) =>
            new BracketOrder(
                this, 
                takeProfitLimitPrice, 
                stopLossStopPrice, 
                stopLossLimitPrice);
    }
}
