using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalQuote : IHistoricalQuote
    {
        [JsonIgnore]
        String IQuoteBase<String>.BidExchange => throw new InvalidOperationException();

        [JsonIgnore]
        String IQuoteBase<String>.AskExchange => throw new InvalidOperationException();

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? ParticipantTimestampUtc { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        [JsonConverter(typeof(UnixNanosecondsDateTimeConverter))]
        public DateTime? TradeReportingFacilityTimestampUtc { get; set; }

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

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public List<Int64>? ConditionsList { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public List<Int64>? IndicatorsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => ConditionsList.EmptyIfNull();

        [JsonIgnore]

        public IReadOnlyList<Int64> Indicators => IndicatorsList.EmptyIfNull();
    }
}
