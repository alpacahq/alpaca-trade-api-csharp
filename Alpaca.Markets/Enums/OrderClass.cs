namespace Alpaca.Markets;

/// <summary>
/// Order class for advanced orders in the Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public enum OrderClass
{
    /// <summary>
    /// Simple order
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "simple")]
    Simple,

    /// <summary>
    /// Bracket order
    /// </summary>
    [EnumMember(Value = "bracket")]
    Bracket,

    /// <summary>
    /// One Cancels Other order
    /// </summary>
    [EnumMember(Value = "oco")]
    OneCancelsOther,

    /// <summary>
    /// One Triggers Other order
    /// </summary>
    [EnumMember(Value = "oto")]
    OneTriggersOther,

    /// <summary>
    /// Multi-leg options order
    /// </summary>
    [EnumMember(Value = "mleg")]
    MultiLegOptions
}
