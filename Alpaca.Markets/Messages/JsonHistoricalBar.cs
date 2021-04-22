using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static class JsonHistoricalBar
    {
        [SuppressMessage(
            "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
            Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
        internal sealed class V1 : IBar
        {
            [JsonIgnore]
            public String Symbol { get; internal set; } = String.Empty;

            [JsonProperty(PropertyName = "o", Required = Required.Default)]
            public Decimal Open { get; set; }

            [JsonProperty(PropertyName = "c", Required = Required.Default)]
            public Decimal Close { get; set; }

            [JsonProperty(PropertyName = "l", Required = Required.Default)]
            public Decimal Low { get; set; }

            [JsonProperty(PropertyName = "h", Required = Required.Default)]
            public Decimal High { get; set; }

            [JsonProperty(PropertyName = "v", Required = Required.Default)]
            public UInt64 Volume { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Default)]
            [JsonConverter(typeof(UnixSecondsDateTimeConverter))]
            public DateTime TimeUtc { get; set; }
        }

        [SuppressMessage(
            "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
            Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
        internal sealed class V2 : IBar
        {
            [JsonIgnore]
            public String Symbol { get; internal set; } = String.Empty;

            [JsonProperty(PropertyName = "o", Required = Required.Always)]
            public Decimal Open { get; set; }

            [JsonProperty(PropertyName = "c", Required = Required.Always)]
            public Decimal Close { get; set; }

            [JsonProperty(PropertyName = "l", Required = Required.Always)]
            public Decimal Low { get; set; }

            [JsonProperty(PropertyName = "h", Required = Required.Always)]
            public Decimal High { get; set; }

            [JsonProperty(PropertyName = "v", Required = Required.Always)]
            public UInt64 Volume { get; set; }

            [JsonProperty(PropertyName = "t", Required = Required.Always)]
            public DateTime TimeUtc { get; set; }
        }
    }
}
