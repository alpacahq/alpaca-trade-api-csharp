using System;

namespace Alpaca.Markets
{
    public interface IPosition
    {
        Guid AccountId { get; }

        Guid AssetId { get; }

        String Symbol { get; }

        Exchange Exchange { get; }

        AssetClass AssetClass { get; }

        Decimal AverageEntryPrice { get; }

        Int32 Quantity { get; }

        PositionSide Side { get; }

        Decimal MarketValue { get; }

        Decimal CostBasis { get; }

        Decimal UnrealizedProfitLoss { get; }

        Decimal UnrealizedProfitLossPercent { get; }

        Decimal IntadayUnrealizedProfitLoss { get; }

        Decimal IntradayUnrealizedProfitLossPercent { get; }

        Decimal AssetCurrentPrice { get; }

        Decimal AssetLastPrice { get; }

        Decimal AssetChangePercent { get; }
    }
}