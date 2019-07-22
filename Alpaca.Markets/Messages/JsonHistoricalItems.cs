using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal class JsonHistoricalItems<TApi, TJson> : IHistoricalItems<TApi> where TJson : TApi
    {
        private static readonly IReadOnlyList<TApi> _empty = new TApi[0];

        [JsonProperty(PropertyName = "status", Required = Required.Default)]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "ticker", Required = Required.Default)]
        private String Ticker{ set { Symbol = value; } }

        [JsonProperty(PropertyName = "adjusted", Required = Required.Default)]
        public Boolean Adjusted { get; set; }

        [JsonProperty(PropertyName = "queryCount", Required = Required.Default)]
        public Int64 QueryCount { get; set; }

        [JsonProperty(PropertyName = "resultsCount", Required = Required.Default)]
        public Int64 ResultsCount { get; set; }

        [JsonProperty(PropertyName = "ticks", Required = Required.Default)]
        public List<TJson> ItemsList { get; set; }

        [JsonProperty(PropertyName = "results", Required = Required.Default)]
        private List<TJson> Results { set { ItemsList = value; } }

        [JsonIgnore]
        public IReadOnlyList<TApi> Items =>
            (IReadOnlyList<TApi>)ItemsList ?? _empty;
    }
}
