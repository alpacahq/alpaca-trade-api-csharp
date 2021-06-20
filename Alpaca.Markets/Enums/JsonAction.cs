using System.Diagnostics.CodeAnalysis;
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

        [EnumMember(Value = "unlisten")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        Unlisten,

        [EnumMember(Value = "auth")]
        StreamingAuthenticate,

        [EnumMember(Value = "subscribe")]
        Subscribe,

        [EnumMember(Value = "unsubscribe")]
        Unsubscribe,
    }
}
