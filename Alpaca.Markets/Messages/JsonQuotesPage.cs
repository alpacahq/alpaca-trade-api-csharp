using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonQuotesPage : IPage<IHistoricalQuote>
    {
        [JsonProperty(PropertyName = "quotes", Required = Required.Always)]
        public List<JsonHistoricalQuote> ItemsList { get; set; } = new List<JsonHistoricalQuote>();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IHistoricalQuote> Items => ItemsList.EmptyIfNull();
    }
}
