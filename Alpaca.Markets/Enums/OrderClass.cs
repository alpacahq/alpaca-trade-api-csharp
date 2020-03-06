using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    /// <summary>
    /// Order class for advanced orders in the Alpaca REST API.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderClass
    {
        /// <summary>
        /// Bracket order
        /// </summary>
        [EnumMember(Value = "bracket")]
        Bracket,

        /// <summary>
        /// One Cancels Other order
        /// </summary>
        [EnumMember(Value = "oco")]
        OneCancelsOther,

        /// <summary>
        /// One Triggers Other order
        /// </summary>
        [EnumMember(Value = "oto")]
        OneTriggersOther,
    }
}
