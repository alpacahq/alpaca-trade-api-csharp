using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Possible crypto taker side types for Alpaca Data API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TakerSide
    {
        /// <summary>
        /// Raw data - no adjustment.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "-")]
        Unknown,

        /// <summary>
        /// Stock split adjustments.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "B")]
        Buy,

        /// <summary>
        /// Stock dividend adjustments.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "S")]
        Sell
    }
}
