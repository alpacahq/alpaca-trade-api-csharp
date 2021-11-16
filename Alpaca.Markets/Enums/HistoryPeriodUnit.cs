using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

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
    /// Month
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
    /// 3 month
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "A")]
    Year
}
