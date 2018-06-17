using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    internal sealed class JsonCalendar : ICalendar
    {
        private sealed class DateConverter : IsoDateTimeConverter
        {
            public DateConverter()
            {
                DateTimeStyles = DateTimeStyles.AssumeLocal;
                DateTimeFormat = "yyyy-MM-dd";
            }
        }

        private sealed class TimeConverter : IsoDateTimeConverter
        {
            public TimeConverter()
            {
                DateTimeStyles = DateTimeStyles.AssumeLocal;
                DateTimeFormat = "HH:mm";
            }
        }

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
                TradingDate.Date, DateTimeKind.Utc);
            TradingOpenTime = TradingDate.Add(
                TradingOpenTime.TimeOfDay);
            TradingCloseTime = TradingDate.Add(
                TradingCloseTime.TimeOfDay);
        }
    }
}