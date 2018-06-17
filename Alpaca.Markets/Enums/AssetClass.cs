using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetClass
    {
        [EnumMember(Value = "us_equity")]
        UsEquity
    }
}