namespace Alpaca.Markets;

/// <summary>
/// Encapsulates portfolio history information item from Alpaca REST API.
/// </summary>
public interface IPortfolioHistoryItem
{
    /// <summary>
    /// Gets historical equity value.
    /// </summary>
    [UsedImplicitly]
    Decimal? Equity { get; }

    /// <summary>
    /// Gets historical profit/loss value.
    /// </summary>
    [UsedImplicitly]
    Decimal? ProfitLoss { get; }

    /// <summary>
    /// Gets historical profit/loss value as percentages.
    /// </summary>
    [UsedImplicitly]
    Decimal? ProfitLossPercentage { get; }

    /// <summary>
    /// Gets historical timestamp value in UTC time zone.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }
}
