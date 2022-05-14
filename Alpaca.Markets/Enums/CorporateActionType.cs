namespace Alpaca.Markets;

/// <summary>
/// Corporate action type in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CorporateActionType
{
    /// <summary>
    /// Dividends payment.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "dividend")]
    Dividend,

    /// <summary>
    /// Merge another symbol.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "merger")]
    Merger,

    /// <summary>
    /// Spin off another symbol.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "spinoff")]
    SpinOff,

    /// <summary>
    /// Split company shares.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "split")]
    Split
}
