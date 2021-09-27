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
    internal sealed class JsonMultiQuotesPage<TQuote> : IMultiPageMutable<IQuote>
        where TQuote : IQuote, ISymbolMutable
    {
        [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
        public Dictionary<String, List<TQuote>?> ItemsDictionary { get; set; } = new ();

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore]
        public IReadOnlyDictionary<String, IReadOnlyList<IQuote>> Items { get; set; } =
            new Dictionary<String, IReadOnlyList<IQuote>>();
            
        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Items = ItemsDictionary.EmptyIfNull<IQuote, TQuote>(
                (symbol, list) => list?.ForEach(item => item.SetSymbol(symbol)));
    }
}
