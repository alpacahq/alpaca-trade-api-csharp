namespace Alpaca.Markets;

/// <summary>
/// Encapsulates portfolio history information from Alpaca REST API.
/// </summary>
public interface IPortfolioHistory
{
    /// <summary>
    /// Gets historical information items list with timestamps.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IPortfolioHistoryItem> Items { get; }

    /// <summary>
    /// Gets time frame value for this historical view.
    /// </summary>
    [UsedImplicitly]
    TimeFrame TimeFrame { get; }

    /// <summary>
    /// Gets base value for this historical view.
    /// </summary>
    [UsedImplicitly]
    Decimal BaseValue { get; }
}
