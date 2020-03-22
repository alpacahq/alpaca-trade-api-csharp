using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Historical data aggregation type in Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum AggregationType
    {
        /// <summary>
        /// Aggregated data for single trading day.
        /// </summary>
        [EnumMember(Value = "daily")]
        Day,

        /// <summary>
        /// Aggregated data for single minute.
        /// </summary>
        [EnumMember(Value = "min")]
        Minute
    }
}
