using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonClock : IClock
    {
        [JsonIgnore]
        public DateTime Timestamp => TimestampUtc;

        [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "is_open", Required = Required.Always)]
        public Boolean IsOpen { get; set; }

        [JsonIgnore]
        public DateTime NextOpen => NextOpenUtc;

        [JsonProperty(PropertyName = "next_open", Required = Required.Always)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime NextOpenUtc { get; set; }

        [JsonIgnore]
        public DateTime NextClose => NextCloseUtc;

        [JsonProperty(PropertyName = "next_close", Required = Required.Always)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime NextCloseUtc { get; set; }
    }
}
