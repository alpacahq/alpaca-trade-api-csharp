using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonNewOrderAdvancedAttributes
    {
        [JsonProperty(PropertyName = "limit_price", Required = Required.Default)]
        public Decimal? LimitPrice { get; set; }

        [JsonProperty(PropertyName = "stop_price", Required = Required.Default)]
        public Decimal? StopPrice { get; set; }
    }
}
