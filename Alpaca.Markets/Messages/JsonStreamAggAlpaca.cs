using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonStreamAggAlpaca : IStreamAgg
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "o", Required = Required.Always)]
        public Decimal Open { get; set; }

        [JsonProperty(PropertyName = "h", Required = Required.Always)]
        public Decimal High { get; set; }

        [JsonProperty(PropertyName = "l", Required = Required.Always)]
        public Decimal Low { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Always)]
        public Decimal Close { get; set; }

        [JsonProperty(PropertyName = "a", Required = Required.Default)]
        public Decimal Average { get; set; }

        [JsonProperty(PropertyName = "v", Required = Required.Always)]
        public Int64 Volume { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        [JsonConverter(typeof(UnixMillisecondsDateTimeConverter))]
        public DateTime StartTimeUtc { get; set; }

        [JsonProperty(PropertyName = "e", Required = Required.Always)]
        [JsonConverter(typeof(UnixMillisecondsDateTimeConverter))]
        public DateTime EndTimeUtc { get; set; }

        [JsonProperty(PropertyName = "n", Required = Required.Default)]
        public Int32 ItemsInWindow { get; set; }
    }
}
