using System;

namespace Alpaca.Markets
{
    public interface IStreamQuote
    {
        String Symbol { get; }

        Int64 BidExchange { get; }

        Int64 AskExchange { get; }

        Decimal BidPrice { get; }

        Decimal AskPrice { get; }

        Int64 BidSize { get; }

        Int64 AskSize { get; }

        DateTime Time { get; }
    }
}