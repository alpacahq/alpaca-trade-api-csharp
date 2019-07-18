using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonUnsubscribeRequest
    {
        [JsonProperty(PropertyName = "action", Required = Required.Always)]
        public JsonAction Action { get; set; }

        [JsonProperty(PropertyName = "params", Required = Required.Default)]
        public String Params { get; set; }
    }
}
