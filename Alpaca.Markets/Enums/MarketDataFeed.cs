namespace Alpaca.Markets;

/// <summary>
/// Supported market feed data types for Alpaca Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum MarketDataFeed
{
    /// <summary>
    /// IEX feed - the only option available for the free data (no subscription).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "iex")]
    Iex,

    /// <summary>
    /// SIP feed - option available for the subscribed clients.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "sip")]
    Sip,

    /// <summary>
    /// OTC feed - option available for the subscribed clients.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "otc")]
    Otc,

    /// <summary>
    /// DelayedSIP feed - 15-minute delayed <see cref="Sip"/>. It can only be used
    /// in the latest endpoints and on the stream. For historical endpoints you can
    /// simply use <see cref="Sip"/> and set the end parameter to 15 minutes ago.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "delayed_sip")]
    DelayedSip,

    /// <summary>
    /// BOATS feed - Blue Ocean, overnight US trading data.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "boats")]
    Boats,

    /// <summary>
    /// Overnight feed - derived overnight US trading data.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "overnight")]
    Overnight
}
