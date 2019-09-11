using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonPositionActionStatus : IPositionActionStatus
    {
        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; }

        [JsonIgnore]
        public Boolean IsSuccess => StatusCode.IsSuccessHttpStatusCode();

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public Int64 StatusCode { get; set; }
    }
}