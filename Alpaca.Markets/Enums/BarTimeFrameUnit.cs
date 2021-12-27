using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
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
        Day
    }
}
