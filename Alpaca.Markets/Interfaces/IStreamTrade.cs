using System;

namespace Alpaca.Markets
{
    public interface IStreamTrade
    {
        String Symbol { get; }

        Int64 Exchange { get; }

        Decimal Price { get; }

        Int64 Size { get; }

        DateTime Time { get; }
    }
}