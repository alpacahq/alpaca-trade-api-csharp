using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAggHistoricalItems<TApi, TJson>
        : JsonHistoricalItems<TApi, TJson>, IAggHistoricalItems<TApi> where TJson : TApi
    {
        [JsonProperty(PropertyName = "aggType", Required = Required.Always)]
        public AggregationType AggregationType { get; set; }
    }
}
