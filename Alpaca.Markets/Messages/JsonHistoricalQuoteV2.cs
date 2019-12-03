using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal class JsonHistoricalQuoteV2 : IHistoricalQuoteV2
    {
        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 SipTimestamp { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        public Int64 ParticipantTimestamp { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        public Int64 TrfTimestamp { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public IReadOnlyList<Int64> Conditions { get; set; }

        [JsonProperty(PropertyName = "X", Required = Required.Default)]
        public Int64 AskExchange { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        public Int64 BidExchange { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        public Int64 BidSize { get; set; }

        [JsonProperty(PropertyName = "S", Required = Required.Default)]
        public Int64 AskSize { get; set; }

        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "P", Required = Required.Default)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public IReadOnlyList<Int64> Indicators { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }
    }
}
