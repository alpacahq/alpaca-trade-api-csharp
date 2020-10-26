using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonStreamTradePolygon : IStreamTrade
    {
        [JsonProperty(PropertyName = "sym", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String TradeId { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public Int64 Exchange { get; set; }

        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        [JsonConverter(typeof(UnixMillisecondsDateTimeConverter))]
        public DateTime TimeUtc { get; set; }
    }
}
