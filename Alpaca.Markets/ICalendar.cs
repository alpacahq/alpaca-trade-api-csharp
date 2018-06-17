using System;

namespace Alpaca.Markets
{
    public interface ICalendar
    {
        DateTime TradingDate { get; }

        DateTime TradingOpenTime { get; }

        DateTime TradingCloseTime { get; }
    }
}