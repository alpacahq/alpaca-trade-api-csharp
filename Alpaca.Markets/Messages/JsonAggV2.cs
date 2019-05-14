using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonAggV2 : JsonAggBase
    {

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        [OnDeserialized]
        internal override void OnDeserializedMethod(
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