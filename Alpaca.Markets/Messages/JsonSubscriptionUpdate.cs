using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonSubscriptionUpdate
    {
        [JsonProperty(PropertyName = "action", Required = Required.Default)]
        public JsonAction Action { get; set; }

        [JsonProperty(PropertyName = "trades", Required = Required.Always)]
        public List<String> Trades { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "quotes", Required = Required.Always)]
        public List<String> Quotes { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "bars", Required = Required.Always)]
        public List<String> MinuteBars { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "dailyBars", Required = Required.Always)]
        public List<String> DailyBars { get; set; } = new List<String>();
    }
}
