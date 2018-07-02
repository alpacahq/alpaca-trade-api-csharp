using System;

namespace Alpaca.Markets
{
    public interface ITradeUpdate
    {
        String Event { get; }

        Decimal? Price { get; }

        Int64? Quantity { get; }

        DateTime Timestamp { get; }

        IOrder Order { get; }
    }
}