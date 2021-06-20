using System;
using JetBrains.Annotations;

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
            Decimal limitPrice)
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
        [UsedImplicitly]
        public Decimal StopPrice { get; }

        /// <summary>
        /// Gets or sets the new order limit price.
        /// </summary>
        [UsedImplicitly]
        public Decimal LimitPrice { get; set; }
                
        /// <summary>
        /// Creates new buy stop limit order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static StopLimitOrder Buy(
            String symbol,
            Int64 quantity,
            Decimal stopPrice,
            Decimal limitPrice) =>
            new (
                symbol, quantity, OrderSide.Buy, stopPrice, limitPrice);

        /// <summary>
        /// Creates new sell stop limit order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static StopLimitOrder Sell(
            String symbol,
            Int64 quantity,
            Decimal stopPrice,
            Decimal limitPrice) =>
            new (
                symbol, quantity, OrderSide.Sell, stopPrice, limitPrice);

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithLimitPrice(LimitPrice)
                .WithStopPrice(StopPrice);
    }
}
