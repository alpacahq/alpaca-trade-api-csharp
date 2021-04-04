using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Supported bar duration for Alpaca Data API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BarTimeFrame
    {
        /// <summary>
        /// One minute bars.
        /// </summary>
        [EnumMember(Value = "1Sec")]
        [Obsolete("This enum member will be removed in the next major SDK release.", true)]
        Second,

        /// <summary>
        /// One minute bars.
        /// </summary>
        [EnumMember(Value = "1Min")]
        Minute,

        /// <summary>
        /// Hour bars.
        /// </summary>
        [EnumMember(Value = "1Hour")]
        Hour,

        /// <summary>
        /// Daily bars.
        /// </summary>
        [EnumMember(Value = "1Day")]
        Day,
    }
}
