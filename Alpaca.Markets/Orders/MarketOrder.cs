using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data required for placing the market order on the Alpaca REST API.
    /// </summary>
    public sealed class MarketOrder : SimpleOrderBase
    {
        internal MarketOrder(
            String symbol,
            Int64 quantity,
            OrderSide side)
            : base(
                symbol, quantity, side,
                OrderType.Market)
        {
        }
    }
}
