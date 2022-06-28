using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Alpaca.Markets
{

    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonOrderBook : IOrderBook
    {
        internal sealed class Entry : IOrderBookEntry
        {
            [JsonProperty(PropertyName = "p", Required = Required.Always)]
            public Decimal Price { get; set; }

            [JsonProperty(PropertyName = "s", Required = Required.Always)]
            public Decimal Size { get; set; }
        }

        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimestampUtc { get; set; }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public String Exchange { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "b", Required = Required.Always)]
        internal List<Entry> BidsList { get; set; } = new();

        [JsonProperty(PropertyName = "a", Required = Required.Always)]
        internal List<Entry> AsksList { get; set; } = new();

        [JsonProperty(PropertyName = "r", Required = Required.Default)]
        public Boolean IsReset { get; set; }

        [JsonIgnore] public IReadOnlyList<IOrderBookEntry> Bids { get; private set; } = new List<IOrderBookEntry>();

        [JsonIgnore] public IReadOnlyList<IOrderBookEntry> Asks { get; private set; } = new List<IOrderBookEntry>();

        [OnDeserialized]
        [UsedImplicitly]
        internal void OnDeserializedMethod(
            StreamingContext _)
        {
            Bids = BidsList.EmptyIfNull();
            Asks = AsksList.EmptyIfNull();
        }
    }
}