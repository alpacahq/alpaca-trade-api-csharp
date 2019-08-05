using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonClock : IClock
    {
        [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "is_open", Required = Required.Always)]
        public Boolean IsOpen { get; set; }

        [JsonProperty(PropertyName = "next_open", Required = Required.Always)]
        public DateTime NextOpen { get; set; }

        [JsonProperty(PropertyName = "next_close", Required = Required.Always)]
        public DateTime NextClose { get; set; }
    }
}
