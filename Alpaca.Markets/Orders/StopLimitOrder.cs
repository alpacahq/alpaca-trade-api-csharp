using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the stop limit order on the Alpaca REST API.
    /// </summary>
    public sealed class StopLimitOrder : SimpleOrderBase
    {
        internal StopLimitOrder(
            String symbol,
            Int64 quantity,
            OrderSide side,
            Decimal stopPrice,
            Decimal limitPrice
        )
            : base(
                symbol, quantity, side,
                OrderType.StopLimit)
        {
            StopPrice = stopPrice;
            LimitPrice = limitPrice;
        }

        /// <summary>
        /// Gets or sets the new order stop price.
        /// </summary>
        public Decimal StopPrice { get; }

        /// <summary>
        /// Gets or sets the new order limit price.
        /// </summary>
        public Decimal LimitPrice { get; set; }

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithLimitPrice(LimitPrice)
                .WithStopPrice(StopPrice);
    }
}
