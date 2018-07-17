using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthStatus
    {
        [EnumMember(Value = "authorized")]
        Authorized,

        [EnumMember(Value = "unauthorized")]
        Unauthorized
    }
}