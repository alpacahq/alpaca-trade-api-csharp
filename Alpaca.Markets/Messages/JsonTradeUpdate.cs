using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonTradeUpdate : ITradeUpdate
    {
        [JsonProperty(PropertyName = "event", Required = Required.Always)]
        public TradeEvent Event { get; set; }

        [JsonProperty(PropertyName = "price", Required = Required.Default)]
        public Decimal? Price { get; set; }

        [JsonProperty(PropertyName = "position_qty", Required = Required.Default)]
        public Int64? Quantity { get; set; }

        [JsonProperty(PropertyName = "timestamp", Required = Required.Default)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public JsonOrder JsonOrder { get; set; } = new JsonOrder();

        [JsonIgnore]
        public IOrder Order => JsonOrder;
    }
}
