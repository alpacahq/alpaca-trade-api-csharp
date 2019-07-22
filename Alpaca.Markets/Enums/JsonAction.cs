using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    internal enum JsonAction
    {
        [EnumMember(Value = "authenticate")]
        Authenticate,

        [EnumMember(Value = "listen")]
        Listen,

        [EnumMember(Value = "auth")]
        PolygonAuthenticate,

        [EnumMember(Value = "subscribe")]
        PolygonSubscribe,

        [EnumMember(Value = "unsubscribe")]
        PolygonUnsubscribe,
    }
}
