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
    internal sealed class JsonMultiBarsPage : IMultiPageMutable<IBar>
    {
        [JsonProperty(PropertyName = "bars", Required = Required.Default)]
        public Dictionary<String,List<JsonHistoricalBar>?> ItemsDictionary { get; set; } = new ();

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<String,IReadOnlyList<IBar>> Items { get; set; } =
            new Dictionary<String, IReadOnlyList<IBar>>();
            
        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Items = ItemsDictionary.EmptyIfNull<IBar, JsonHistoricalBar>(
                (symbol, list) => list?.ForEach(item => item.Symbol = symbol));
    }
}
