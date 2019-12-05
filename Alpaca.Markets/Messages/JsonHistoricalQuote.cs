using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    // TODO: OlegRa - remove `V1` class and flatten hierarchy after removing Polygon Historical API v1 support

    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalQuote : IHistoricalQuote
    {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        private static readonly IReadOnlyList<Int64> _empty = new Int64[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.

        [JsonIgnore]
        String IQuoteBase<String>.BidExchange => throw new InvalidOperationException();

        [JsonIgnore]
        String IQuoteBase<String>.AskExchange => throw new InvalidOperationException();

        public Int64 TimeOffset => throw new InvalidOperationException();

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimestampInNanoseconds { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        public Int64 ParticipantTimestampInNanoseconds { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        public Int64 TradeReportingFacilityTimestampInNanoseconds { get; set; }

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
        public List<Int64> ConditionsList { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public List<Int64> IndicatorsList { get; set; }

        [JsonIgnore]
        public DateTime Timestamp =>
            DateTimeHelper.FromUnixTimeNanoseconds(TimestampInNanoseconds);

        [JsonIgnore]
        public DateTime ParticipantTimestamp =>
            DateTimeHelper.FromUnixTimeNanoseconds(ParticipantTimestampInNanoseconds);

        [JsonIgnore]
        public DateTime TradeReportingFacilityTimestamp =>
            DateTimeHelper.FromUnixTimeNanoseconds(TradeReportingFacilityTimestampInNanoseconds);

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => ConditionsList ?? _empty;

        [JsonIgnore]

        public IReadOnlyList<Int64> Indicators => IndicatorsList ?? _empty;
    }

    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalQuoteV1 : IHistoricalQuote
    {
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

        public DateTime Timestamp => throw new InvalidOperationException();

        public DateTime ParticipantTimestamp => throw new InvalidOperationException();

        public DateTime TradeReportingFacilityTimestamp => throw new InvalidOperationException();

        Int64 IQuoteBase<Int64>.AskExchange => throw new InvalidOperationException();

        Int64 IQuoteBase<Int64>.BidExchange => throw new InvalidOperationException();

        public Int64 Tape => throw new InvalidOperationException();

        public Int64 SequenceNumber => throw new InvalidOperationException();

        public IReadOnlyList<Int64> Conditions => throw new InvalidOperationException();

        public IReadOnlyList<Int64> Indicators => throw new InvalidOperationException();
    }
}
