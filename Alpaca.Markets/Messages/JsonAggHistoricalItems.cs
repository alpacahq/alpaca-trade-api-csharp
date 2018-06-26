using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed class JsonAggHistoricalItems<TApi, TJson>
        : JsonHistoricalItems<TApi, TJson>, IAggHistoricalItems<TApi> where TJson : TApi
    {
        [JsonProperty(PropertyName = "aggType", Required = Required.Always)]
        public AggregationType AggregationType { get; set; }
    }
}