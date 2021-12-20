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
    internal sealed class JsonQuotesPage<TQuote> : IPageMutable<IQuote>
        where TQuote : IQuote, ISymbolMutable
    {
        [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
        public List<TQuote> ItemsList { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IQuote> Items { get; set; } = new List<IQuote>();
            
        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context) =>
            Items = ItemsList.SetSymbol(Symbol).EmptyIfNull<IQuote, TQuote>();
    }
}
