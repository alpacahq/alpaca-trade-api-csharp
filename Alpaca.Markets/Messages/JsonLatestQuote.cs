using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestQuote : IRealTimeQuote
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        internal sealed class Quote
        {
            [JsonProperty(PropertyName = "bx", Required = Required.Always)]
            public String BidExchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "ax", Required = Required.Always)]
            public String AskExchange { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "bp", Required = Required.Always)]
            public Decimal BidPrice { get; set; }

            [JsonProperty(PropertyName = "ap", Required = Required.Always)]
            public Decimal AskPrice { get; set; }

            [JsonProperty(PropertyName = "bs", Required = Required.Always)]
            public UInt64 BidSize { get; set; }

            [JsonProperty(PropertyName = "as", Required = Required.Always)]
            public UInt64 AskSize { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimestampUtc { get; set; }

            [JsonProperty(PropertyName = "c", Required = Required.Default)]
            public List<String> ConditionsList { get; } = new ();
        }

        [JsonProperty(PropertyName = "quote", Required = Required.Always)]
        public Quote Nested { get; set; } = new Quote();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public DateTime TimestampUtc => Nested.TimestampUtc;

        [JsonIgnore]
        public String AskExchange => Nested.AskExchange;

        [JsonIgnore]
        public String BidExchange => Nested.BidExchange;

        [JsonIgnore]
        public Decimal BidPrice => Nested.BidPrice;

        [JsonIgnore]
        public Decimal AskPrice => Nested.AskPrice;

        [JsonIgnore]
        public UInt64 BidSize => Nested.BidSize;

        [JsonIgnore]
        public UInt64 AskSize => Nested.AskSize;
    
        [JsonIgnore]
        public IReadOnlyList<String> Conditions =>
            Nested.ConditionsList.EmptyIfNull();
    }
}
