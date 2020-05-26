using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the stop order on the Alpaca REST API.
    /// </summary>
    public sealed class StopOrder : SimpleOrderBase
    {
        internal StopOrder(
            String symbol,
            Int64 quantity,
            OrderSide side,
            Decimal stopPrice
        )
            : base(
                symbol, quantity, side,
                OrderType.Stop) =>
            StopPrice = stopPrice;

        /// <summary>
        /// Gets or sets the new order stop price.
        /// </summary>
        public Decimal StopPrice { get; }
        
        /// <summary>
        /// Creates new buy stop order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
        public static StopOrder Buy(
            String symbol,
            Int64 quantity,
            Decimal stopPrice) =>
            new StopOrder(
                symbol, quantity, OrderSide.Buy, stopPrice);

        /// <summary>
        /// Creates new sell buy order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
        public static StopOrder Sell(
            String symbol,
            Int64 quantity,
            Decimal stopPrice) =>
            new StopOrder(
                symbol, quantity, OrderSide.Sell, stopPrice);

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithStopPrice(StopPrice);
    }
}
