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
    internal sealed class JsonMultiTradesPage : IMultiPageMutable<ITrade>
    {
        [JsonProperty(PropertyName = "trades", Required = Required.Default)]
        public Dictionary<String, List<JsonHistoricalTrade>?> ItemsDictionary { get; set; } = new ();

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore] 
        public IReadOnlyDictionary<String, IReadOnlyList<ITrade>> Items { get; set; } =
            new Dictionary<String, IReadOnlyList<ITrade>>();
            
        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Items = ItemsDictionary.EmptyIfNull<ITrade, JsonHistoricalTrade>(
            (symbol, list) => list?.ForEach(item => item.SetSymbol(symbol)));
    }
}
