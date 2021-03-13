using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAlpacaHistoricalQuote : IHistoricalQuote
    {
        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "ax", Required = Required.Always)]
        public String AskExchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "ap", Required = Required.Default)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "as", Required = Required.Default)]
        public Int64 AskSize { get; set; }

        [JsonProperty(PropertyName = "bx", Required = Required.Always)]
        public String BidExchange  { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "bp", Required = Required.Default)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "bs", Required = Required.Default)]
        public Int64 BidSize { get; set; }

        [JsonIgnore]
        Int64 IQuoteBase<Int64>.AskExchange => throw new InvalidOperationException();

        [JsonIgnore]
        Int64 IQuoteBase<Int64>.BidExchange => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 Tape => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 SequenceNumber => throw new InvalidOperationException();

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => throw new InvalidOperationException();

        [JsonIgnore]

        public IReadOnlyList<Int64> Indicators => throw new InvalidOperationException();
    }
}
