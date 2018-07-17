using System;

namespace Alpaca.Markets
{
    public interface IHistoricalTrade
    {
        String Exchange { get; }

        Int64 TimeOffset  { get; }

        Decimal Price { get; }

        Int64 Size { get; }
    }
}