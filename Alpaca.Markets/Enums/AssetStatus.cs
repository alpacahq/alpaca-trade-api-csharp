namespace Alpaca.Markets;

/// <summary>
/// Single asset status in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AssetStatus
{
    /// <summary>
    /// Active asset.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "active")]
    Active,

    /// <summary>
    /// Inactive asset.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "inactive")]
    Inactive,

    /// <summary>
    /// Delisted asset.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "delisted")]
    Delisted
}
