using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonAuthResponse
    {
        [JsonProperty(PropertyName = "action", Required = Required.Always)]
        public String Action { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public AuthStatus Status { get; set; }

        [JsonProperty(PropertyName = "message", Required = Required.Default)]
        public String Message { get; set; } = String.Empty;
    }
}
