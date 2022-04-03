using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{

    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonCorrection : ICorrection, ITrade
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public String Exchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public String Tape { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "oi", Required = Required.Default)]
        public UInt64 TradeId { get; set; }

        [JsonProperty(PropertyName = "op", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "os", Required = Required.Always)]
        public Decimal Size { get; set; }

        [JsonProperty(PropertyName = "oc", Required = Required.Default)]
        public List<String> ConditionsList { get; set; } = new();

        [JsonProperty(PropertyName = "ci", Required = Required.Default)]
        public UInt64 CorrectedTradeId { get; set; }

        [JsonProperty(PropertyName = "cp", Required = Required.Always)]
        public Decimal CorrectedPrice { get; set; }

        [JsonProperty(PropertyName = "cs", Required = Required.Always)]
        public Decimal CorrectedSize { get; set; }

        [JsonProperty(PropertyName = "cc", Required = Required.Default)]
        public List<String> CorrectedConditionsList { get; set; } = new();

        [JsonProperty(PropertyName = "tks", Required = Required.Default)]
        public TakerSide TakerSide { get; set; } = TakerSide.Unknown;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions =>
            ConditionsList.EmptyIfNull();

        [JsonIgnore] public ITrade OriginalTrade => this;

        [JsonIgnore] public ITrade CorrectedTrade { get; private set; } = new JsonRealTimeTrade();

        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            CorrectedTrade = new JsonRealTimeTrade
            {
                Tape = Tape,
                Symbol = Symbol,
                Channel = Channel,
                Exchange = Exchange,
                TimestampUtc = TimestampUtc,
                Size = CorrectedSize,
                Price = CorrectedPrice,
                TradeId = CorrectedTradeId,
                ConditionsList = CorrectedConditionsList
            };
    }
}
