using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonSubscriptionUpdate
    {
        [JsonProperty(PropertyName = "action", Required = Required.Default)]
        public JsonAction Action { get; set; }

        [JsonProperty(PropertyName = "trades", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<String>? Trades { get; set; }

        [JsonProperty(PropertyName = "quotes", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<String>? Quotes { get; set; }

        [JsonProperty(PropertyName = "bars", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public List<String>? Bars { get; set; }
    }
}
