using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the limit order on the Alpaca REST API.
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
        /// Creates a new instance of the <see cref="OneCancelsOtherOrder"/> order from the current order.
        /// </summary>
        /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
        /// <param name="stopLossLimitPrice">Stop loss order limit price (optional).</param>
        /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
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
