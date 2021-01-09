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
        /// Creates new buy market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
        public static LimitOrder Buy(
            String symbol,
            Int64 quantity,
            Decimal limitPrice) =>
            new LimitOrder(
                symbol, quantity, OrderSide.Buy, limitPrice);

        /// <summary>
        /// Creates new sell market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
        public static LimitOrder Sell(
            String symbol,
            Int64 quantity,
            Decimal limitPrice) =>
            new LimitOrder(
                symbol, quantity, OrderSide.Sell, limitPrice);

        /// <summary>
        /// Creates a new instance of the <see cref="OneCancelsOtherOrder"/> order from the current order.
        /// </summary>
        /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
        /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
        public OneCancelsOtherOrder OneCancelsOther(
            Decimal stopLossStopPrice) =>
            new OneCancelsOtherOrder(
                this,
                stopLossStopPrice,
                null);

        /// <summary>
        /// Creates a new instance of the <see cref="OneCancelsOtherOrder"/> order from the current order.
        /// </summary>
        /// <param name="stopLossStopPrice">Stop loss order stop price.</param>
        /// <param name="stopLossLimitPrice">Stop loss order limit price.</param>
        /// <returns>New advanced order representing pair of original order and stop loss order.</returns>
        public OneCancelsOtherOrder OneCancelsOther(
            Decimal stopLossStopPrice,
            Decimal stopLossLimitPrice) =>
            new OneCancelsOtherOrder(
                this,
                stopLossStopPrice,
                stopLossLimitPrice);

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithLimitPrice(LimitPrice);
    }
}
