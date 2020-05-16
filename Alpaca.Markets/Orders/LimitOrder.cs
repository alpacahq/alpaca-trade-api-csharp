using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LimitOrder : SimpleOrderBase
    {
        internal LimitOrder(
            String symbol,
            Int64 quantity,
            OrderSide side,
            Decimal limitPrice)
            : base(
                symbol, quantity, side,
                OrderType.Limit) =>
            LimitPrice = limitPrice;

        /// <summary>
        /// Gets or sets the new order limit price.
        /// </summary>
        public Decimal LimitPrice { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OneCancelsOtherOrder OneCancelsOther(
            Decimal stopLossStopPrice,
            Decimal? stopLossLimitPrice = null) =>
            new OneCancelsOtherOrder(
                this,
                stopLossStopPrice,
                stopLossLimitPrice);

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithLimitPrice(LimitPrice);
    }
}
