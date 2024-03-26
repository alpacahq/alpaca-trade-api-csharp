namespace Alpaca.Markets;

/// <summary>
/// Supported options feed for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OptionsFeed
{
    /// <summary>
    /// Options Price Reporting Authority.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "opra")]
    Opra,

    /// <summary>
    /// Indicative options data.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "indicative")]
    Indicative
}
