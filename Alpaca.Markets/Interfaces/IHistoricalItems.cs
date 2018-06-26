using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    public interface IHistoricalItems<out TItem>
    {
        String Status { get; }

        String Symbol { get; }

        IReadOnlyCollection<TItem> Items { get; }
    }
}