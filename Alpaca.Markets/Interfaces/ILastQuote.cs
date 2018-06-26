using System;

namespace Alpaca.Markets
{
    public interface ILastQuote
    {
        String Status { get; }

        String Symbol { get; }

        String BidExchange { get; }

        String AskExchange { get; }

        Decimal BidPrice { get; }

        Decimal AskPrice { get; }

        Int64 BidSize { get; }

        Int64 AskSize { get; }

        DateTime Time { get; }
    }
}