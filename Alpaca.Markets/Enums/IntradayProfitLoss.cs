namespace Alpaca.Markets;

/// <summary>
/// Intraday profit/loss calculation for portfolio history in the Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum IntradayProfitLoss
{
    /// <summary>
    /// Don't reset the profit/los value to the previous day's closing equity for each trading day.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "no_reset")]
    NoReset,

    /// <summary>
    /// Reset the profit/los value to the previous day's closing equity for each trading day.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "per_day")]
    PerDay
}
