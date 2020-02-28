using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Timeframe values for portfolio history in the Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum HistoryTimeframe
    {
        /// <summary>
        /// 1 minute
        /// </summary>
        [EnumMember(Value = "1Min")]
        Minute,

        /// <summary>
        /// 5 minute
        /// </summary>
        [EnumMember(Value = "5Min")]
        FiveMinute,

        /// <summary>
        /// 15 minute
        /// </summary>
        [EnumMember(Value = "15Min")]
        FifteenMinute,

        /// <summary>
        /// 1 hour
        /// </summary>
        [EnumMember(Value = "1H")]
        Hour,

        /// <summary>
        /// 1 day
        /// </summary>
        [EnumMember(Value = "1D")]
        Day,
    }
}
