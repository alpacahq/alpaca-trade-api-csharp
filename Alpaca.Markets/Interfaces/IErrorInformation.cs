namespace Alpaca.Markets;

/// <summary>
/// Encapsulates extended error information from Alpaca error REST response.
/// </summary>
public interface IErrorInformation
{
    /// <summary>
    /// Gets the symbol name related for the error.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the number of opened orders if this value related to the error.
    /// </summary>
    [UsedImplicitly]
    Int32? OpenOrdersCount { get; }

    /// <summary>
    /// Gets the day trading buying power if this value related to the error.
    /// </summary>
    [UsedImplicitly]
    Decimal? DayTradingBuyingPower { get; }

    /// <summary>
    /// Gets the maximal day trading buying power if this value related to the error.
    /// </summary>
    [UsedImplicitly]
    Decimal? MaxDayTradingBuyingPowerUsed { get; }

    /// <summary>
    /// Gets the used maximal day trading buying power if this value related to the error.
    /// </summary>
    [UsedImplicitly]
    Decimal? MaxDayTradingBuyingPowerUsedSoFar { get; }
}
