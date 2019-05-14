using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal abstract class JsonHistoricalItemsV2<TApi, TJson> : IHistoricalItemsV2<TApi> where TJson : TApi
    {
        private static readonly IReadOnlyList<TApi> _empty = new TApi[0];

        [JsonProperty(PropertyName = "status", Required = Required.Default)]
        public String Status { get; set; }

        [JsonProperty(PropertyName = "ticker", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonProperty(PropertyName = "results", Required = Required.Always)]
        public List<TJson> ItemsList { get; set; }

        [JsonProperty(PropertyName = "adjusted", Required = Required.Always)]
        public Boolean Adjusted { get; set; }

        [JsonProperty(PropertyName = "queryCount", Required = Required.Always)]
        public Int64 QueryCount { get; set; }

        [JsonProperty(PropertyName = "resultsCount", Required = Required.Always)]
        public Int64 ResultsCount { get; set; }

        [JsonIgnore]
        public IReadOnlyList<TApi> Items =>
            (IReadOnlyList<TApi>)ItemsList ?? _empty;
    }
}