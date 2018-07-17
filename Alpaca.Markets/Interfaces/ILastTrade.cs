using System;

namespace Alpaca.Markets
{
    public interface ILastTrade
    {
        String Status { get; }

        String Symbol { get; }

        Int64 Exchange { get; }

        Decimal Price { get; }

        Int64 Size { get; }

        DateTime Time { get; }
    }
}