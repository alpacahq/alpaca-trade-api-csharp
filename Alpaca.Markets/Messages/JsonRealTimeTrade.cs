using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonRealTimeTrade : ITrade
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public UInt64 TradeId { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public String Exchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public String Tape { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public Decimal Size { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public List<String> ConditionsList { get; } = new ();

        [JsonProperty(PropertyName = "tks", Required = Required.Default)]
        public TakerSide TakerSide { get; set; } = TakerSide.Unknown;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions => 
            ConditionsList.EmptyIfNull();
    }
}
