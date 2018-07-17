using System;

namespace Alpaca.Markets
{
    public interface IStreamBar
    {
        String Symbol { get; }

        Int64 Exchange { get; }

        Decimal Open { get; }

        Decimal High { get; }

        Decimal Low { get; }

        Decimal Close { get; }

        Decimal Average { get; }

        Int64 Volume { get; }

        DateTime StartTime { get; }

        DateTime EndTime { get; }
    }
}