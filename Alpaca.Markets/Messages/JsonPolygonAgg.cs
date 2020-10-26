using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonPolygonAgg : IAgg
    {
        [JsonProperty(PropertyName = "o", Required = Required.Default)]
        public Decimal Open { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public Decimal Close { get; set; }

        [JsonProperty(PropertyName = "l", Required = Required.Default)]
        public Decimal Low { get; set; }

        [JsonProperty(PropertyName = "h", Required = Required.Default)]
        public Decimal High { get; set; }

        [JsonProperty(PropertyName = "v", Required = Required.Default)]
        public Int64 Volume { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        [JsonConverter(typeof(UnixMillisecondsDateTimeConverter))]
        public DateTime? TimeUtc { get; set; }

        [JsonProperty(PropertyName = "n", Required = Required.Default)]
        public Int32 ItemsInWindow { get; set; }
    }
}
