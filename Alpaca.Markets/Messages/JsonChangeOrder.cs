using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonChangeOrder
    {
        [JsonProperty(PropertyName = "qty", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Int64? Quantity { get; set; }

        [JsonProperty(PropertyName = "time_in_force", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public TimeInForce? TimeInForce { get; set; }

        [JsonProperty(PropertyName = "limit_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Decimal? LimitPrice { get; set; }

        [JsonProperty(PropertyName = "stop_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public Decimal? StopPrice { get; set; }

        [JsonProperty(PropertyName = "client_order_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String? ClientOrderId { get; set; }
    }
}
