namespace Alpaca.Markets;

/// <summary>
/// Options trading level for Alpaca REST API.
/// </summary>
public enum OptionsTradingLevel
{
    /// <summary>
    /// US options trading completely disabled.
    /// </summary>
    [UsedImplicitly]
    Disabled = 0,

    /// <summary>
    /// Us options trading with covered call / cash-secured put.
    /// </summary>
    [UsedImplicitly]
    CoveredCallCashSecuredPut,

    /// <summary>
    /// US options trading with long call and put support.
    /// </summary>
    [UsedImplicitly]
    LongCallPut
}
