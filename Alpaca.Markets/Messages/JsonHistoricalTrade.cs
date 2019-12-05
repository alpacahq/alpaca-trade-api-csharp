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
    internal class JsonHistoricalTrade : IHistoricalTrade
    {
        
        // V1 API fields

        [JsonProperty(PropertyName = "e", Required = Required.Default)]
        public String Exchange { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        public Int64 Size { get; set; }

        // V2 API fields
        public Int64 SipTimestamp { get { return TimeOffset; } }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        public Int64 ParticipantTimestamp { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        public Int64 TrfTimestamp { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public IReadOnlyList<Int64> Conditions { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        private Int64 ExchangeV2 { set { Exchange = value.ToString(CultureInfo.InvariantCulture); } }

        [JsonProperty(PropertyName = "r", Required = Required.Default)]
        public Int64 TrfId { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String Id { get; set; }

        [JsonProperty(PropertyName = "I", Required = Required.Default)]
        public String OrigId { get; set; }
    }
}
