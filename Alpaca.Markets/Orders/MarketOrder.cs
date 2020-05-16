using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
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
