using System.Runtime.Serialization;
using JetBrains.Annotations;
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
        [UsedImplicitly]
        [EnumMember(Value = "1Min")]
        Minute,

        /// <summary>
        /// Hour bars.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "1Hour")]
        Hour,

        /// <summary>
        /// Daily bars.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "1Day")]
        Day
    }
}
