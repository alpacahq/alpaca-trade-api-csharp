namespace Alpaca.Markets;

/// <summary>
/// Intraday reporting styles for portfolio history in the Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum IntradayReporting
{
    /// <summary>
    /// Only timestamps for the core equity trading hours are returned (usually 9:30am to 4:00pm, trading days only).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "market_hours")]
    MarketHours,

    /// <summary>
    /// Returns timestamps for the whole session including extended hours (usually 4:00am to 8:00pm, trading days only).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "extended_hours")]
    ExtendedHours,

    /// <summary>
    /// Returns price data points 24/7 (for off-session times too). To calculate the equity values we are using the following prices:
    /// - Between 4:00am and 10:00pm on trading days the valuation will be calculated based on the last trade (extended hours and normal hours respectively).
    /// - After 10:00pm, until the next session open the equities will be valued at their official closing price on the primary exchange.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "continuous")]
    Continuous
}
