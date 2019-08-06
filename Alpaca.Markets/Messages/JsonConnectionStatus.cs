using System;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal sealed class JsonConnectionStatus
    {
        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public ConnectionStatus Status { get; set; }

        [JsonProperty(PropertyName = "message", Required = Required.Always)]
        public String Message { get; set; }
    }
}