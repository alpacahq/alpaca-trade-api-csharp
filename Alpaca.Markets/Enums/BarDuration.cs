using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BarDuration
    {
        [EnumMember(Value = "1Min")]
        OneMinute,

        [EnumMember(Value = "5Min")]
        FiveMinutes,

        [EnumMember(Value = "15Min")]
        FifteenMinutes,

        [EnumMember(Value = "1H")]
        OneHour,

        [EnumMember(Value = "1D")]
        OneDay,
    }
}