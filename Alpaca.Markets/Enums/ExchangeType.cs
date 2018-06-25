using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExchangeType
    {
        [EnumMember(Value = "exchange")]
        Exchange,

        [EnumMember(Value = "TRF")]
        TradeReportingFacility
    }
}