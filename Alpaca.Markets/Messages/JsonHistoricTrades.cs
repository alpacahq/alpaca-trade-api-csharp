using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Alpaca.Markets
{
    public sealed class JsonHistoricTrades : IHistoricTrades
    {
        private static readonly TimeZoneInfo _easternTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        private sealed class DateConverter : IsoDateTimeConverter
        {
            public DateConverter()
            {
                DateTimeStyles = DateTimeStyles.AssumeLocal;
                DateTimeFormat = "yyyy-MM-dd";
            }
        }

        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(PropertyName = "day", Required = Required.Always)]
        public DateTime TradesDay { get; set; }

        [JsonProperty(PropertyName = "msLatency", Required = Required.Always)]
        public Int64 LatencyInMs { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "ticks", Required = Required.Always)]
        public List<JsonHistoricTrade> TradesList { get; set; }

        [JsonIgnore]
        public IReadOnlyCollection<IHistoricTrade> Trades => TradesList;

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            TradesDay = DateTime.SpecifyKind(
                TradesDay.Date, DateTimeKind.Utc);

            var estTradingDate = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(TradesDay.Date, DateTimeKind.Utc),
                _easternTimeZone).Date;
        }
    }
}