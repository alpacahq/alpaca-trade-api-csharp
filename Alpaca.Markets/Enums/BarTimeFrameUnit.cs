namespace Alpaca.Markets;

/// <summary>
/// Supported bar duration for Alpaca Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum BarTimeFrameUnit
{
    /// <summary>
    /// Minute bars.
    /// </summary>
    [EnumMember(Value = "Min")]
    Minute,

    /// <summary>
    /// Hour bars.
    /// </summary>
    [EnumMember(Value = "Hour")]
    Hour,

    /// <summary>
    /// Daily bars.
    /// </summary>
    [EnumMember(Value = "Day")]
    Day,
    
    /// <summary>
    /// Weekly bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "Week")]
    Week,

    /// <summary>
    /// Monthly bars.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "Month")]
    Month
}
