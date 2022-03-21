using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the market order on the Alpaca REST API.
    /// </summary>
    public sealed class MarketOrder : SimpleOrderBase
    {
        internal MarketOrder(
            String symbol,
            OrderQuantity quantity,
            OrderSide side)
            : base(
                symbol, quantity.Value.AsInteger(), side,
                OrderType.Market) =>
            Quantity = quantity;

        /// <summary>
        /// Gets the market orders quantity as fractional or notional value.
        /// </summary>
        [UsedImplicitly]
        public new OrderQuantity Quantity { get; }

        /// <summary>
        /// Creates new buy market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static MarketOrder Buy(
            String symbol,
            OrderQuantity quantity) =>
            new (
                symbol, quantity, OrderSide.Buy);

        /// <summary>
        /// Creates new sell market order using specified symbol and quantity.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <returns>The new <see cref="MarketOrder"/> object instance.</returns>
        [UsedImplicitly]
        public static MarketOrder Sell(
            String symbol,
            OrderQuantity quantity) =>
            new (
                symbol, quantity, OrderSide.Sell);

        internal override Boolean IsQuantityValid() =>
            Quantity.Value > 0M;

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest().WithQuantity(Quantity);
    }
}
