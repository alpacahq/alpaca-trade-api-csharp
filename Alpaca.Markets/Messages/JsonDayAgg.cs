using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonDayAgg : JsonAggBase
    {

        [JsonConverter(typeof(DateConverter), "yyyy-M-d")]
        [JsonProperty(PropertyName = "d", Required = Required.Default)]
        new public DateTime Time { get; set; }

        [OnDeserialized]
        internal override void OnDeserializedMethod(
            StreamingContext context)
        {
            Time = DateTime.SpecifyKind(
                Time.Date, DateTimeKind.Utc);
        }
    }
}