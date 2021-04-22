﻿using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestTrade : IStreamTrade
    {
        internal sealed class Trade
        {
            [JsonProperty(PropertyName = "i", Required = Required.Default)]
            public String TradeId { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "x", Required = Required.Always)]
            public String Exchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "p", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "s", Required = Required.Always)]
            public Int64 Size { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimeUtc { get; set; }
        }

        [JsonProperty(PropertyName = "trade", Required = Required.Always)]
        public Trade Nested { get; set; } = new Trade();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public String TradeId => Nested.TradeId;

        [JsonIgnore]
        public String Exchange => Nested.Exchange;

        [JsonIgnore]
        public Decimal Price => Nested.Price;

        [JsonIgnore]
        public Int64 Size => Nested.Size;

        [JsonIgnore]
        public DateTime TimeUtc => Nested.TimeUtc;
    }
}