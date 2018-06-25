using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarketDataType
    {
        [EnumMember(Value = "equities")]
        Equities,

        [EnumMember(Value = "indecies")]
        Indexes
    }
}