using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Exchange
    {
        [EnumMember(Value = "NYSEMKT")]
        NyseMkt,

        [EnumMember(Value = "NYSEARCA")]
        NyseArca,

        [EnumMember(Value = "NYSE")]
        Nyse,

        [EnumMember(Value = "NASDAQ")]
        Nasdaq
    }
}