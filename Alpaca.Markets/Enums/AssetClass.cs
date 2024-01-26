namespace Alpaca.Markets;

/// <summary>
/// Supported asset classes for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AssetClass
{
    /// <summary>
    /// US equity asset class.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "us_equity")]
    UsEquity,

    /// <summary>
    /// Cryptocurrency asset class.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "crypto")]
    Crypto,

    /// <summary>
    /// US option asset class.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "us_option")]
    UsOption
}
