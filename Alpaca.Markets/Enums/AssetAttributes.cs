namespace Alpaca.Markets;

/// <summary>
/// Unique asset characteristics for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AssetAttributes
{
    /// <summary>
    /// PTP asset accepting orders without exception.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ptp_no_exception")]
    PtpNoException,

    /// <summary>
    /// PTP asset accepting orders with exception.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ptp_with_exception")]
    PtpWithException,

    /// <summary>
    /// IPO asset, only limit orders are allowed prior to an IPO being traded on secondary market
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ipo")]
    // ReSharper disable once InconsistentNaming
    IPO
}
