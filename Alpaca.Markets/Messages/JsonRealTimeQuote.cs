using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonRealTimeQuote : IQuote
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "bx", Required = Required.Always)]
        public String BidExchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "ax", Required = Required.Always)]
        public String AskExchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "bp", Required = Required.Always)]
        public Decimal BidPrice { get; set; }

        [JsonProperty(PropertyName = "ap", Required = Required.Always)]
        public Decimal AskPrice { get; set; }

        [JsonProperty(PropertyName = "bs", Required = Required.Always)]
        public Decimal BidSize { get; set; }

        [JsonProperty(PropertyName = "as", Required = Required.Always)]
        public Decimal AskSize { get; set; }

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "c", Required = Required.Default)]
        public List<String> ConditionsList { get; } = new ();

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public String Tape { get; set; } = String.Empty;

        [JsonIgnore]
        public IReadOnlyList<String> Conditions => 
            ConditionsList.EmptyIfNull();
    }
}
