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
        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "o", Required = Required.Always)]
        public Decimal Open { get; set; }

        [JsonProperty(PropertyName = "h", Required = Required.Always)]
        public Decimal High { get; set; }

        [JsonProperty(PropertyName = "l", Required = Required.Always)]
        public Decimal Low { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Always)]
        public Decimal Close { get; set; }

        [JsonIgnore]
        public Decimal Average => throw new InvalidOperationException();

        [JsonProperty(PropertyName = "v", Required = Required.Always)]
        public Int64 Volume { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime StartTimeUtc { get; set; }

        [JsonIgnore]
        public DateTime EndTimeUtc => StartTimeUtc;

        [JsonIgnore]
        public Int32 ItemsInWindow => throw new InvalidOperationException();
    }
}
