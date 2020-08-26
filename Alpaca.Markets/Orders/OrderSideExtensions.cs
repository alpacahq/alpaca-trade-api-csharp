using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Set of extensions methods for creating the <see cref="OrderBase"/> inheritors.
    /// </summary>
    public static class OrderSideExtensions
    {
        /// <summary>
        /// Creates new market order using specified side, symbol, and quantity.
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
        public static MarketOrder Market(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity) =>
            new MarketOrder(symbol, quantity, orderSide);

        /// <summary>
        /// Creates new stop order using specified side, symbol, quantity, and stop price.
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <returns>The new <see cref="StopOrder"/> object instance.</returns>
        public static StopOrder Stop(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal stopPrice) =>
            new StopOrder(symbol, quantity, orderSide, stopPrice);

        /// <summary>
        /// Creates new limit order using specified side, symbol, quantity, and limit price.
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="LimitOrder"/> object instance.</returns>
        public static LimitOrder Limit(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal limitPrice) =>
            new LimitOrder(symbol, quantity, orderSide, limitPrice);

        /// <summary>
        /// Creates new limit order using specified side, symbol, quantity, stop, and limit prices.
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <returns>The new <see cref="StopLimitOrder"/> object instance.</returns>
        public static StopLimitOrder StopLimit(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal stopPrice,
            Decimal limitPrice) =>
            new StopLimitOrder(symbol, quantity, orderSide, stopPrice, limitPrice);

        /// <summary>
        /// Creates new trailing stop order using specified side, symbol, quantity, and trail offset.
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="trailOffset">Order trail offset.</param>
        /// <returns>The new <see cref="TrailingStopOrder"/> object instance.</returns>
        public static TrailingStopOrder TrailingStop(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            TrailOffset trailOffset) =>
            new TrailingStopOrder(symbol, quantity, orderSide, trailOffset);
    }
}
