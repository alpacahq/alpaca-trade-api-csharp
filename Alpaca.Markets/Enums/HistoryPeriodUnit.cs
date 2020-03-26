using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Period units for portfolio history in the Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum HistoryPeriodUnit
    {
        /// <summary>
        /// Day
        /// </summary>
        [EnumMember(Value = "D")]
        Day,

        /// <summary>
        /// Month
        /// </summary>
        [EnumMember(Value = "W")]
        Week,

        /// <summary>
        /// Month
        /// </summary>
        [EnumMember(Value = "M")]
        Month,

        /// <summary>
        /// 3 month
        /// </summary>
        [EnumMember(Value = "A")]
        Year
    }
}
