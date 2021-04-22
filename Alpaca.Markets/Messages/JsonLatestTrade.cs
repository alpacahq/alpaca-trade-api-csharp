using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestTrade : IRealTimeTrade
    {
        internal sealed class Trade
        {
            [JsonProperty(PropertyName = "i", Required = Required.Default)]
            public UInt64 TradeId { get; set; }

            [JsonProperty(PropertyName = "x", Required = Required.Always)]
            public String Exchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "z", Required = Required.Default)]
            public String Tape { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "p", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "s", Required = Required.Always)]
            public UInt64 Size { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimestampUtc { get; set; }

            [JsonProperty(PropertyName = "c", Required = Required.Default)]
            public List<String> ConditionsList { get; } = new ();
        }

        [JsonProperty(PropertyName = "trade", Required = Required.Always)]
        public Trade Nested { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public DateTime TimestampUtc => Nested.TimestampUtc;

        [JsonIgnore]
        public UInt64 TradeId => Nested.TradeId;

        [JsonIgnore]
        public String Tape => Nested.Tape;

        [JsonIgnore]
        public String Exchange => Nested.Exchange;

        [JsonIgnore]
        public Decimal Price => Nested.Price;

        [JsonIgnore]
        public UInt64 Size => Nested.Size;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions =>
            Nested.ConditionsList.EmptyIfNull();
    }
}
