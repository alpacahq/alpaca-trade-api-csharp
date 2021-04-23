using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLatestQuote : IStreamQuote
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
            public Int64 BidSize { get; set; }

            [JsonProperty(PropertyName = "as", Required = Required.Always)]
            public Int64 AskSize { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimeUtc { get; set; }
        }

        [JsonProperty(PropertyName = "quote", Required = Required.Always)]
        public Quote Nested { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonIgnore]
        Int64 IQuoteBase<Int64>.BidExchange => throw new InvalidOperationException();

        [JsonIgnore]
        Int64 IQuoteBase<Int64>.AskExchange => throw new InvalidOperationException();

        [JsonIgnore]
        public String AskExchange => Nested.AskExchange;

        [JsonIgnore]
        public String BidExchange => Nested.BidExchange;

        [JsonIgnore]
        public Decimal BidPrice => Nested.BidPrice;

        [JsonIgnore]
        public Decimal AskPrice => Nested.AskPrice;

        [JsonIgnore]
        public Int64 BidSize => Nested.BidSize;

        [JsonIgnore]
        public Int64 AskSize => Nested.AskSize;

        [JsonIgnore]
        public DateTime TimeUtc => Nested.TimeUtc;
    }
}
