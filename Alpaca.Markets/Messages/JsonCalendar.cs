using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonCalendar : ICalendar
    {
        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(PropertyName = "date", Required = Required.Always)]
        public DateTime TradingDate { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "open", Required = Required.Always)]
        public DateTime TradingOpenTime { get; set; }

        [JsonConverter(typeof(TimeConverter))]
        [JsonProperty(PropertyName = "close", Required = Required.Always)]
        public DateTime TradingCloseTime { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            TradingDate = DateTime.SpecifyKind(
                TradingDate.Date, DateTimeKind.Unspecified).Date;

#if NETSTANDARD1_6
            TradingOpenTime = TimeZoneInfo.ConvertTime(
                TradingDate.Add(TradingOpenTime.TimeOfDay),
                CustomTimeZone.Est, TimeZoneInfo.Utc);
            TradingCloseTime = TimeZoneInfo.ConvertTime(
                TradingDate.Add(TradingCloseTime.TimeOfDay),
                CustomTimeZone.Est, TimeZoneInfo.Utc);
#else
            TradingOpenTime = TimeZoneInfo.ConvertTimeToUtc(
                TradingDate.Add(TradingOpenTime.TimeOfDay),
                CustomTimeZone.Est);
            TradingCloseTime = TimeZoneInfo.ConvertTimeToUtc(
                TradingDate.Add(TradingCloseTime.TimeOfDay),
                CustomTimeZone.Est);
#endif
            TradingDate = DateTime.SpecifyKind(
                TradingDate.Date, DateTimeKind.Utc);
        }
    }
}