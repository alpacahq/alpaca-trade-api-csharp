using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates position information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IPosition
    {
        /// <summary>
        /// Gets unique account identifier.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release", true)]
        Guid AccountId { get; }

        /// <summary>
        /// Gets unique asset identifier.
        /// </summary>
        Guid AssetId { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets asset exchange.
        /// </summary>
        Exchange Exchange { get; }

        /// <summary>
        /// Gets asset class.
        /// </summary>
        AssetClass AssetClass { get; }

        /// <summary>
        /// Gets average entry price for position.
        /// </summary>
        Decimal AverageEntryPrice { get; }

        /// <summary>
        /// Get position quantity (with the fractional part).
        /// </summary>
        Decimal Quantity { get; }

        /// <summary>
        /// Get position quantity (rounded to the nearest integer).
        /// </summary>
        Int64 IntegerQuantity { get; }

        /// <summary>
        /// Get position side (short or long).
        /// </summary>
        PositionSide Side { get; }

        /// <summary>
        /// Get current position market value.
        /// </summary>
        Decimal MarketValue { get; }

        /// <summary>
        /// Get position cost basis.
        /// </summary>
        Decimal CostBasis { get; }

        /// <summary>
        /// Get position unrealized profit loss.
        /// </summary>
        Decimal UnrealizedProfitLoss { get; }

        /// <summary>
        /// Get position unrealized profit loss in percent.
        /// </summary>
        Decimal UnrealizedProfitLossPercent { get; }

        /// <summary>
        /// Get position intraday unrealized profit loss.
        /// </summary>
        Decimal IntradayUnrealizedProfitLoss { get; }

        /// <summary>
        /// Get position intraday unrealized profit loss in percent.
        /// </summary>
        Decimal IntradayUnrealizedProfitLossPercent { get; }

        /// <summary>
        /// Gets position's asset current price.
        /// </summary>
        Decimal AssetCurrentPrice { get; }

        /// <summary>
        /// Gets position's asset last trade price.
        /// </summary>
        Decimal AssetLastPrice { get; }

        /// <summary>
        /// Gets position's asset price change in percent.
        /// </summary>
        Decimal AssetChangePercent { get; }
    }
}
