using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLastQuoteAlpaca : ILastQuote
    {
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        internal struct Last
        {
            [JsonProperty(PropertyName = "bidexchange", Required = Required.Always)]
            public Int64 BidExchange { get; set; }

            [JsonProperty(PropertyName = "askexchange", Required = Required.Always)]
            public Int64 AskExchange { get; set; }

            [JsonProperty(PropertyName = "bidprice", Required = Required.Default)]
            public Decimal BidPrice { get; set; }

            [JsonProperty(PropertyName = "askprice", Required = Required.Default)]
            public Decimal AskPrice { get; set; }

            [JsonProperty(PropertyName = "bidsize", Required = Required.Default)]
            public Int64 BidSize { get; set; }

            [JsonProperty(PropertyName = "asksize", Required = Required.Default)]
            public Int64 AskSize { get; set; }

            [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
            public Int64 Timestamp { get; set; }
        }

        [JsonProperty(PropertyName = "last", Required = Required.Always)]
        public Last Nested { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public String Status { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        public Int64 BidExchange => Nested.BidExchange;

        [JsonIgnore]
        public Int64 AskExchange => Nested.AskExchange;

        [JsonIgnore]
        public Decimal BidPrice => Nested.BidPrice;

        [JsonIgnore]
        public Decimal AskPrice => Nested.AskPrice;

        [JsonIgnore]
        public Int64 BidSize => Nested.BidSize;

        [JsonIgnore]
        public Int64 AskSize => Nested.AskSize;

        [JsonIgnore]
        public DateTime Time => 
            DateTimeHelper.FromUnixTimeNanoseconds(Nested.Timestamp);

        [JsonIgnore]
        public DateTime TimeUtc => 
            DateTimeHelper.FromUnixTimeNanoseconds(Nested.Timestamp);
    }
}
