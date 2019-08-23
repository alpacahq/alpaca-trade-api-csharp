using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonExchange : IExchange
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Int64 ExchangeId { get; set; }

        [JsonProperty(PropertyName = "type", Required = Required.Always)]
        public ExchangeType ExchangeType { get; set; }

        [JsonProperty(PropertyName = "market", Required = Required.Always)]
        public MarketDataType MarketDataType { get; set; }

        [JsonProperty(PropertyName = "mic", Required = Required.Default)]
        public String MarketIdentificationCode { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "tape", Required = Required.Default)]
        public String TapeId { get; set; }
    }
}
