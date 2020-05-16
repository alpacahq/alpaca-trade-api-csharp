using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrderSideExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <returns></returns>
        public static MarketOrder Market(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity) =>
            new MarketOrder(symbol, quantity, orderSide);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice"></param>
        /// <returns></returns>
        public static StopOrder Stop(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal stopPrice) =>
            new StopOrder(symbol, quantity, orderSide, stopPrice);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="limitPrice"></param>
        /// <returns></returns>
        public static LimitOrder Limit(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal limitPrice) =>
            new LimitOrder(symbol, quantity, orderSide, limitPrice);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderSide">Order side (buy or sell).</param>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="stopPrice"></param>
        /// <param name="limitPrice"></param>
        /// <returns></returns>
        public static StopLimitOrder StopLimit(
            this OrderSide orderSide,
            String symbol,
            Int64 quantity,
            Decimal stopPrice,
            Decimal limitPrice) =>
            new StopLimitOrder(symbol, quantity, orderSide, stopPrice, limitPrice);
    }
}
