using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonStreamTrade : IStreamTrade
    {
        [JsonProperty(PropertyName = "sym", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Int64 Exchange { get; set; }

        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public Int64 Timestamp { get; set; }

        [JsonIgnore]
        public DateTime Time { get; private set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
#if NET45
            Time = DateTimeHelper.FromUnixTimeMilliseconds(Timestamp);
#else
            Time = DateTime.SpecifyKind(
                DateTimeOffset.FromUnixTimeMilliseconds(Timestamp)
                    .DateTime, DateTimeKind.Utc);
#endif
        }
    }
}