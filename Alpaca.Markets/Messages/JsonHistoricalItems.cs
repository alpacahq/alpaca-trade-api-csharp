using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonHistoricalItems<TApi, TJson> : IHistoricalItems<TApi> where TJson : TApi
    {
        [JsonProperty(PropertyName = "status", Required = Required.Default)]
        public String? Status { get; set; }

        [JsonProperty(PropertyName = "ticker", Required = Required.Default)]
        public String? Symbol { get; set; }

        [JsonProperty(PropertyName = "results", Required = Required.Default)]
        public List<TJson>? ItemsList { get; set; }

        [JsonProperty(PropertyName = "adjusted", Required = Required.Default)]
        public Boolean Adjusted { get; set; }

        [JsonProperty(PropertyName = "queryCount", Required = Required.Default)]
        public Int64 QueryCount { get; set; }

        [JsonProperty(PropertyName = "resultsCount", Required = Required.Default)]
        public Int64 ResultsCount { get; set; }

        [JsonIgnore]
        public IReadOnlyList<TApi> Items => ItemsList.EmptyIfNull<TApi, TJson>();
    }
}
