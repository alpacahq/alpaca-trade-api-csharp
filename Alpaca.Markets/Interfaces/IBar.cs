using System;

namespace Alpaca.Markets
{
    public interface IBar
    {
        Decimal Open { get; }

        Decimal High { get; }

        Decimal Low { get; }

        Decimal Close { get; }

        Int64 Volume { get; }

        DateTime Time { get; }
    }
}