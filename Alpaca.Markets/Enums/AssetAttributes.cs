namespace Alpaca.Markets;

/// <summary>
/// Unique asset characteristics for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(AssetAttributesEnumConverter))]
public enum AssetAttributes
{
    /// <summary>
    /// Unknown asset attribute.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "-")]
    Unknown,

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
    Ipo,

    /// <summary>
    /// Asset supports options trading.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "options_enabled")]
    OptionsEnabled,

    /// <summary>
    /// Asset supports fractional trading at extended hours.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "fractional_eh_enabled")]
    FractionalAtExtended
}
