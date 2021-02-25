using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAlpacaHistoricalTrade : IHistoricalTrade
    {
        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime? TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Default)]
        public String Exchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public Int64 Size { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public String? TradeId { get; set; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public Int64 Tape { get; set; }

        [JsonIgnore]
        public DateTime? ParticipantTimestampUtc => TimestampUtc;

        [JsonIgnore] 
        public DateTime? TradeReportingFacilityTimestampUtc => TimestampUtc;

        [JsonIgnore] 
        public Int64 ExchangeId => throw new InvalidOperationException();

        [JsonIgnore] 
        public Int64 TradeReportingFacilityId => throw new InvalidOperationException();

        [JsonIgnore] 
        public Int64 SequenceNumber => throw new InvalidOperationException();

        [JsonIgnore] 
        public String? OriginalTradeId => throw new InvalidOperationException();

        [JsonIgnore]
        public IReadOnlyList<Int64> Conditions => throw new InvalidOperationException();
    }
}