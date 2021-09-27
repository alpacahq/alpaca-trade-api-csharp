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
        /// Unspecified crypto trade take side.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "-")]
        Unknown,

        /// <summary>
        /// Buy crypto trade take side.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "B")]
        Buy,

        /// <summary>
        /// Bell crypto trade take side.
        /// </summary>
        [UsedImplicitly]
        [EnumMember(Value = "S")]
        Sell
    }
}
