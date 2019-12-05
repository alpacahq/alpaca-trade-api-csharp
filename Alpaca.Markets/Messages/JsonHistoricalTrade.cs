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
    internal sealed class JsonHistoricalTrade : IHistoricalTrade
    {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        private static readonly IReadOnlyList<Int64> _empty = new Int64[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.

        [JsonIgnore]
        public String Exchange  => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 TimeOffset  => throw new InvalidOperationException();

        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimestampInNanoseconds { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Default)]
        public Int64 ParticipantTimestampInNanoseconds { get; set; }

        [JsonProperty(PropertyName = "f", Required = Required.Default)]
        public Int64 TradeReportingFacilityTimestampInNanoseconds { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        public Int64 ExchangeId { get; set; }

        [JsonProperty(PropertyName = "r", Required = Required.Default)]
        public Int64 TradeReportingFacilityId { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonProperty(PropertyName = "q", Required = Required.Default)]
        public Int64 SequenceNumber { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String TradeId { get; set; }

        [JsonProperty(PropertyName = "I", Required = Required.Default)]
        public String OriginalTradeId { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public List<Int64> ConditionsList { get; set; }
        
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
    }

    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalTradeV1 : IHistoricalTrade
    {
        [JsonProperty(PropertyName = "e", Required = Required.Default)]
        public String Exchange { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Default)]
        public Int64 TimeOffset { get; set; }

        [JsonProperty(PropertyName = "p", Required = Required.Default)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Default)]
        public Int64 Size { get; set; }

        [JsonIgnore]
        public DateTime Timestamp => throw new InvalidOperationException();

        [JsonIgnore]
        public DateTime ParticipantTimestamp => throw new InvalidOperationException();

        [JsonIgnore]
        public DateTime TradeReportingFacilityTimestamp => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 ExchangeId => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 TradeReportingFacilityId => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 Tape => throw new InvalidOperationException();

        [JsonIgnore]
        public Int64 SequenceNumber => throw new InvalidOperationException();

        [JsonIgnore]
        public String TradeId => throw new InvalidOperationException();

        [JsonIgnore]
        public String OriginalTradeId => throw new InvalidOperationException();

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => throw new InvalidOperationException();
    }
}
