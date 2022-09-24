namespace Alpaca.Markets;

/// <summary>
/// Encapsulates position information from Alpaca REST API.
/// </summary>
public interface IPosition
{
    /// <summary>
    /// Gets unique asset identifier.
    /// </summary>
    [UsedImplicitly]
    Guid AssetId { get; }

    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets asset exchange.
    /// </summary>
    [UsedImplicitly]
    Exchange Exchange { get; }

    /// <summary>
    /// Gets asset class.
    /// </summary>
    [UsedImplicitly]
    AssetClass AssetClass { get; }

    /// <summary>
    /// Gets average entry price for position.
    /// </summary>
    [UsedImplicitly]
    Decimal AverageEntryPrice { get; }

    /// <summary>
    /// Get position quantity (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal Quantity { get; }

    /// <summary>
    /// Get position quantity (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64 IntegerQuantity { get; }

    /// <summary>
    /// Get total number of shares available minus open orders (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal AvailableQuantity { get; }

    /// <summary>
    /// Get total number of shares available minus open orders (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64 IntegerAvailableQuantity { get; }

    /// <summary>
    /// Get position side (short or long).
    /// </summary>
    [UsedImplicitly]
    PositionSide Side { get; }

    /// <summary>
    /// Get current position market value.
    /// </summary>
    [UsedImplicitly]
    Decimal? MarketValue { get; }

    /// <summary>
    /// Get position cost basis.
    /// </summary>
    [UsedImplicitly]
    Decimal CostBasis { get; }

    /// <summary>
    /// Get position unrealized profit loss.
    /// </summary>
    [UsedImplicitly]
    Decimal? UnrealizedProfitLoss { get; }

    /// <summary>
    /// Get position unrealized profit loss in percent.
    /// </summary>
    [UsedImplicitly]
    Decimal? UnrealizedProfitLossPercent { get; }

    /// <summary>
    /// Get position intraday unrealized profit loss.
    /// </summary>
    [UsedImplicitly]
    Decimal? IntradayUnrealizedProfitLoss { get; }

    /// <summary>
    /// Get position intraday unrealized profit loss in percent.
    /// </summary>
    [UsedImplicitly]
    Decimal? IntradayUnrealizedProfitLossPercent { get; }

    /// <summary>
    /// Gets position's asset current price.
    /// </summary>
    [UsedImplicitly]
    Decimal? AssetCurrentPrice { get; }

    /// <summary>
    /// Gets position's asset last trade price.
    /// </summary>
    [UsedImplicitly]
    Decimal? AssetLastPrice { get; }

    /// <summary>
    /// Gets position's asset price change in percent.
    /// </summary>
    [UsedImplicitly]
    Decimal? AssetChangePercent { get; }
}
