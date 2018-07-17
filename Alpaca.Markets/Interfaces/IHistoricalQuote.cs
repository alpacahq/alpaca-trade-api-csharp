using System;

namespace Alpaca.Markets
{
    public interface IHistoricalQuote
    {
        String BidExchange { get; }

        String AskExchange { get; }

        Int64 TimeOffset { get; }

        Decimal BidPrice { get; }

        Decimal AskPrice { get; }

        Int64 BidSize { get; }

        Int64 AskSize { get; }
    }
}