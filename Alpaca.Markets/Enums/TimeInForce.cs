using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeInForce
    {
        [EnumMember(Value = "day")]
        Day,

        [EnumMember(Value = "gtc")]
        Gtc,

        [EnumMember(Value = "opg")]
        Opg
    }
}