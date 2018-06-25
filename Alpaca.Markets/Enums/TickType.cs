using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TickType
    {
        [EnumMember(Value = "trades")]
        Trades,

        [EnumMember(Value = "quotes")]
        Quotes
    }
}
