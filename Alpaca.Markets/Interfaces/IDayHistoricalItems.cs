using System;

namespace Alpaca.Markets
{
    public interface IDayHistoricalItems<out TItem> : IHistoricalItems<TItem>
    {
        DateTime ItemsDay { get; }

        Int64 LatencyInMs { get; }
    }
}