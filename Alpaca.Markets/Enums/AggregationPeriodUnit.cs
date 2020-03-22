using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported aggregation time windows for Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum AggregationPeriodUnit
    {
        /// <summary>
        /// One minute window
        /// </summary>
        [EnumMember(Value = "minute")]
        Minute,

        /// <summary>
        /// One hour window
        /// </summary>
        [EnumMember(Value = "hour")]
        Hour,

        /// <summary>
        /// One day window
        /// </summary>
        [EnumMember(Value = "day")]
        Day,

        /// <summary>
        /// One week window
        /// </summary>
        [EnumMember(Value = "week")]
        Week,

        /// <summary>
        /// One month window
        /// </summary>
        [EnumMember(Value = "month")]
        Month,

        /// <summary>
        /// One quarter window
        /// </summary>
        [EnumMember(Value = "quarter")]
        Quarter,

        /// <summary>
        /// One year window
        /// </summary>
        [EnumMember(Value = "year")]
        Year,
    }
}
