using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonDayHistoricalItems<TApi, TJson>
        : JsonHistoricalItems<TApi, TJson> , IDayHistoricalItems<TApi> where TJson : TApi
    {
        [JsonConverter(typeof(DateConverter))]
        [JsonProperty(PropertyName = "day", Required = Required.Always)]
        public DateTime ItemsDay { get; set; }

        [JsonProperty(PropertyName = "msLatency", Required = Required.Always)]
        public Int64 LatencyInMs { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            ItemsDay = DateTime.SpecifyKind(
                ItemsDay.Date, DateTimeKind.Utc);
    }
}
