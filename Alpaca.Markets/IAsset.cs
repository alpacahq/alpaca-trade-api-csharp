using System;

namespace Alpaca.Markets
{
    public interface IAsset
    {
        Guid AssetId { get; }

        String Name { get; }

        AssetClass Class { get; }

        Exchange Exchange { get; }

        String Symbol { get; }

        AssetStatus Status { get; }

        Boolean IsTradable { get; }
    }
}