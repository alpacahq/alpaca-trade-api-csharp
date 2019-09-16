﻿using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonChangeOrder
    {
        [JsonProperty(PropertyName = "qty", Required = Required.Default)]
        public Int64? Quantity { get; set; }

        [JsonProperty(PropertyName = "time_in_force", Required = Required.Default)]
        public TimeInForce? TimeInForce { get; set; }

        [JsonProperty(PropertyName = "limit_price", Required = Required.Default)]
        public Decimal? LimitPrice { get; set; }

        [JsonProperty(PropertyName = "stop_price", Required = Required.Default)]
        public Decimal? StopPrice { get; set; }

        [JsonProperty(PropertyName = "client_order_id", Required = Required.Default)]
        public String ClientOrderId { get; set; }
    }
}
