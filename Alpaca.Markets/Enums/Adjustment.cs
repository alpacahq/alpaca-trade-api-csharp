namespace Alpaca.Markets;

/// <summary>
/// Supported bar corporate action adjustment types for Alpaca Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum Adjustment
{
    /// <summary>
    /// Raw data - no adjustment.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "raw")]
    Nothing,

    /// <summary>
    /// Stock split adjustments.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "split")]
    SplitsOnly,

    /// <summary>
    /// Stock dividend adjustments.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "dividend")]
    DividendsOnly,

    /// <summary>
    /// Stock splits and dividend adjustments.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "all")]
    SplitsAndDividends
}
