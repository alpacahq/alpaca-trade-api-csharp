using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base data for ordinal order types, never used directly by any code.
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
        /// Creates a new instance of the <see cref="TakeProfitOrder"/> order from the current order.
        /// </summary>
        /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
        /// <returns>New advanced order representing pair of original order and take profit order.</returns>
        public TakeProfitOrder TakeProfit(
            Decimal takeProfitLimitPrice) =>
            new TakeProfitOrder(
                this, 
                takeProfitLimitPrice);

        /// <summary>
        /// Creates a new instance of the <see cref="StopLossOrder"/> order from the current order.
        /// </summary>
        /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
        /// <param name="stopLossLimitPrice">Stop loss order limit price (optional).</param>
        /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
        public StopLossOrder StopLoss(
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice = null) =>
            new StopLossOrder(
                this, 
                stopLossStopPrice,
                stopLossLimitPrice);

        /// <summary>
        /// Creates a new instance of the <see cref="BracketOrder"/> order from the current order.
        /// </summary>
        /// <param name="takeProfitLimitPrice">Take profit order limit price.</param>
        /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
        /// <param name="stopLossLimitPrice">Stop loss order limit price (optional).</param>
        /// <returns>New advanced order representing an original order plus pair of take profit and stop loss orders.</returns>
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
