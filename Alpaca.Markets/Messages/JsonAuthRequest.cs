using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonAuthRequest
    {
        internal sealed class JsonData
        {
            [JsonProperty(PropertyName = "key_id", Required = Required.Always)]
            public String KeyId { get; set; } = String.Empty;

            [JsonProperty(PropertyName = "secret_key", Required = Required.Always)]
            public String SecretKey { get; set; } = String.Empty;
        }

        [JsonProperty(PropertyName = "action", Required = Required.Always)]
        public JsonAction Action { get; set; }

        [JsonProperty(PropertyName = "data", Required = Required.Default)]
        public JsonData? Data { get; set; }

        [JsonProperty(PropertyName = "params", Required = Required.Default)]
        public String? Params { get; set; }
    }
}
