using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonListeningUpdate
    {
        [JsonProperty(PropertyName = "streams", Required = Required.Default)]
        public List<String> Streams { get; set; } = new List<String>();

        [JsonProperty(PropertyName = "error", Required = Required.Default)]
        public String Error { get; set; } = String.Empty;
    }
}
