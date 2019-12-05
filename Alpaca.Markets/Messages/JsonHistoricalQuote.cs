using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalQuote : IHistoricalQuote
    {

        // V1 API fields

        [JsonProperty(PropertyName = "bE", Required = Required.Default)]
        public String BidExchange { get; set; }

        [JsonProperty(PropertyName = "aE", Required = Required.Default)]
        public String AskExchange { get; set; }

        [JsonProperty(PropertyName = "bP", Required = Required.Default)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "aP", Required = Required.Default)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "bS", Required = Required.Default)]
        public Int64 BidSize { get; set; }

        [JsonProperty(PropertyName = "aS", Required = Required.Default)]
        public Int64 AskSize { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        // V2 API fields
        public Int64 SipTimestamp { get { return TimeOffset; } }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        public Int64 ParticipantTimestamp { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        public Int64 TrfTimestamp { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public IReadOnlyList<Int64> Conditions { get; set; }

        [JsonProperty(PropertyName = "X", Required = Required.Default)]
        private Int64 AskExchangeV2 { set { AskExchange = value.ToString(CultureInfo.InvariantCulture); } }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        private Int64 BidExchangeV2 { set { BidExchange = value.ToString(CultureInfo.InvariantCulture); } }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        private Int64 BidSizeV2 { set { BidSize = value; } }

        [JsonProperty(PropertyName = "S", Required = Required.Default)]
        private Int64 AskSizeV2 { set { AskSize = value; } }

        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        private Decimal BidPriceV2 { set { BidPrice = value; } }

        [JsonProperty(PropertyName = "P", Required = Required.Default)]
        private Decimal AskPriceV2 { set { AskPrice = value; } }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public IReadOnlyList<Int64> Indicators { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }
    }
}
