using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonStreamTradeAlpaca : IStreamTrade
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String TradeId { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public String Exchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public UInt64 Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimeUtc { get; set; }
    }
}
