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
        public Decimal? PositionQuantity { get; set; }

        [JsonIgnore]
        public Int64? PositionIntegerQuantity => PositionQuantity?.AsInteger();

        [JsonProperty(PropertyName = "qty", Required = Required.Default)]
        public Decimal? TradeQuantity { get; set; }

        [JsonIgnore]
        public Int64? TradeIntegerQuantity => TradeQuantity?.AsInteger();

        [JsonProperty(PropertyName = "timestamp", Required = Required.Default)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime? TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "order", Required = Required.Always)]
        public JsonOrder JsonOrder { get; set; } = new ();

        [JsonIgnore]
        public IOrder Order => JsonOrder;

        [JsonIgnore]
        public Decimal? Quantity => PositionQuantity;

        [JsonIgnore]
        public Int64? IntegerQuantity => PositionIntegerQuantity;
    }
}
