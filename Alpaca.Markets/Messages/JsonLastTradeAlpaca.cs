using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLastTradeAlpaca : ILastTrade
    {
        internal struct Last
        {
            [JsonProperty(PropertyName = "exchange", Required = Required.Always)]
            public Int64 Exchange { get; set; }

            [JsonProperty(PropertyName = "price", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "size", Required = Required.Always)]
            public Int64 Size { get; set; }

            [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
            [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
            public DateTime Timestamp { get; set; }
        }

        [JsonProperty(PropertyName = "last", Required = Required.Always)]
        public Last Nested { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public String Status { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public Int64 Exchange => Nested.Exchange;

        [JsonIgnore]
        public Decimal Price => Nested.Price;

        [JsonIgnore]
        public Int64 Size => Nested.Size;

        [JsonIgnore] 
        public DateTime Time => Nested.Timestamp;

        [JsonIgnore]
        public DateTime TimeUtc => Nested.Timestamp;
    }
}
