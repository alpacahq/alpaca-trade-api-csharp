using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AggregationType
    {
        [EnumMember(Value = "daily")]
        Day,

        [EnumMember(Value = "min")]
        Minute
    }
}