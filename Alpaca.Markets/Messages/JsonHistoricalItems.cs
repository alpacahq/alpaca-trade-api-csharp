using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed class JsonHistoricalItems<TApi, TJson>
        : IHistoricalItems<TApi> where TJson : TApi
    {
        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(PropertyName = "day", Required = Required.Always)]
        public DateTime ItemsDay { get; set; }

        [JsonProperty(PropertyName = "msLatency", Required = Required.Always)]
        public Int64 LatencyInMs { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "ticks", Required = Required.Always)]
        public List<TJson> TradesList { get; set; }

        [JsonIgnore]
        public IReadOnlyCollection<TApi> Items =>
            (IReadOnlyCollection<TApi>)TradesList;

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            ItemsDay = DateTime.SpecifyKind(
                ItemsDay.Date, DateTimeKind.Utc);

            var estTradingDate = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.SpecifyKind(ItemsDay.Date, DateTimeKind.Utc),
                CustomTimeZone.Est).Date;
        }
    }
}