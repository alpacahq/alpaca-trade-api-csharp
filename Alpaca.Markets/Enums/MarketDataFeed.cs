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
    Otc
}
