namespace Alpaca.Markets;

/// <summary>
/// Period units for portfolio history in the Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum HistoryPeriodUnit
{
    /// <summary>
    /// Day
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "D")]
    Day,

    /// <summary>
    /// Week
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "W")]
    Week,

    /// <summary>
    /// Month
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "M")]
    Month,

    /// <summary>
    /// Year
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "A")]
    Year
}
