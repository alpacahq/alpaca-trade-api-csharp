using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonLimitUpLimitDown : ILimitUpLimitDown
    {
        [JsonProperty(PropertyName = "T", Required = Required.Always)]
        public String Channel { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "S", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "t", Required = Required.Always)]
        public DateTime TimeUtc { get; set; }

        [JsonProperty(PropertyName = "u", Required = Required.Default)]
        public Decimal LimitUpPrice { get; }

        [JsonProperty(PropertyName = "d", Required = Required.Default)]
        public Decimal LimitDownPrice { get; }

        [JsonProperty(PropertyName = "i", Required = Required.Default)]
        public String Indicator { get; } = String.Empty;

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public String Tape { get; } = String.Empty;
    }
}
