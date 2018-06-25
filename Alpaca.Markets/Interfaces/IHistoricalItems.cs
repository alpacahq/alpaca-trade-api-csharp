using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    public interface IHistoricalItems<TItem>
    {
        DateTime ItemsDay { get; }

        Int64 LatencyInMs { get; }

        String Status { get; }

        String Symbol { get; }

        IReadOnlyCollection<TItem> Items { get; }
    }
}