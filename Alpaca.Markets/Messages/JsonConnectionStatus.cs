using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonConnectionStatus
    {
        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public ConnectionStatus Status { get; set; }

        [JsonProperty(PropertyName = "message", Required = Required.Always)]
        public String Message { get; set; }
    }
}
