using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    public interface IHistoricTrades
    {
        DateTime TradesDay { get; }

        Int64 LatencyInMs { get; }

        String Status { get; }

        String Symbol { get; }

        IReadOnlyCollection<IHistoricTrade> Trades { get; }
    }
}