using System;

namespace Alpaca.Markets
{
    public interface IQuote
    {
        Guid AssetId { get; }

        String Symbol { get; }

        AssetClass AssetClass { get; }

        Decimal BidPrice { get; }

        DateTime BidTime { get; }

        Decimal AskPrice { get; }

        DateTime AskTime { get; }

        Decimal LastPrice { get; }

        DateTime LastTime { get; }

        Decimal DayChange { get; }
    }
}