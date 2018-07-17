using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    public interface IAssetBars
    {
        Guid AssetId { get; }

        String Symbol { get; }

        Exchange Exchange { get; }

        AssetClass AssetClass { get; }

        IReadOnlyList<IBar> Items { get; }
    }
}