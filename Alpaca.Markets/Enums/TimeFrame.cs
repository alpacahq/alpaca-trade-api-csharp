namespace Alpaca.Markets;

/// <summary>
/// Supported bar duration for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TimeFrame
{
    /// <summary>
    /// One minute bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "1Min")]
    Minute,

    /// <summary>
    /// Five minutes bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "5Min")]
    FiveMinutes,

    /// <summary>
    /// Fifteen minutes bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "15Min")]
    FifteenMinutes,

    /// <summary>
    /// Daily bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "1D")]
    Day
}
