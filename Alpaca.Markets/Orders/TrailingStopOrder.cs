using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the market order on the Alpaca REST API.
    /// </summary>
    public sealed class TrailingStopOrder : SimpleOrderBase
    {
        internal TrailingStopOrder(
            String symbol,
            Int64 quantity,
            OrderSide side,
            TrailOffset trailOffset)
            : base(
                symbol, quantity, side,
                OrderType.TrailingStop) =>
            TrailOffset = trailOffset;

        /// <summary>
        /// Gets order trail offset value (in dollars or percents).
        /// </summary>
        [UsedImplicitly]
        public TrailOffset TrailOffset { get; }

        /// <summary>
        /// Creates new buy market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="trailOffset">Trailing stop order offset.</param>
        /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static TrailingStopOrder Buy(
            String symbol,
            Int64 quantity,
            TrailOffset trailOffset) =>
            new (
                symbol, quantity, OrderSide.Buy, trailOffset);

        /// <summary>
        /// Creates new sell market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="trailOffset">Trailing stop order offset.</param>
        /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static TrailingStopOrder Sell(
            String symbol,
            Int64 quantity,
            TrailOffset trailOffset) =>
            new(
                symbol, quantity, OrderSide.Sell, trailOffset);

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithTrailOffset(TrailOffset);
    }
}
