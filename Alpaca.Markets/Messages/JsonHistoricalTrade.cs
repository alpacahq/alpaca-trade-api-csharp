using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalTrade : IHistoricalTrade
    {
        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? ParticipantTimestampUtc { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? TradeReportingFacilityTimestampUtc { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        public Int64 ExchangeId { get; set; }

        [JsonProperty(PropertyName = "r", Required = Required.Default)]
        public Int64 TradeReportingFacilityId { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String? TradeId { get; set; }

        [JsonProperty(PropertyName = "I", Required = Required.Default)]
        public String? OriginalTradeId { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public List<Int64>? ConditionsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => ConditionsList.EmptyIfNull();
    }
}
