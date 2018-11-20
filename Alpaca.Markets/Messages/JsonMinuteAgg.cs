using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonMinuteAgg : IAgg
    {
        [JsonProperty(PropertyName = "o", Required = Required.Always)]
        public Decimal Open { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Always)]
        public Decimal Close { get; set; }

        [JsonProperty(PropertyName = "l", Required = Required.Always)]
        public Decimal Low { get; set; }

        [JsonProperty(PropertyName = "h", Required = Required.Always)]
        public Decimal High { get; set; }

        [JsonProperty(PropertyName = "v", Required = Required.Always)]
        public Int64 Volume { get; set; }

        [JsonProperty(PropertyName = "d", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        [JsonIgnore]
        public DateTime Time { get; private set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
#if NET45
            Time = DateTimeHelper.FromUnixTimeMilliseconds(TimeOffset);
#else
            Time = DateTime.SpecifyKind(
                DateTimeOffset.FromUnixTimeMilliseconds(TimeOffset)
                    .DateTime, DateTimeKind.Utc);
#endif
        }
    }
}