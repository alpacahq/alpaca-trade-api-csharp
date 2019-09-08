using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonOrderActionStatus : IOrderActionStatus
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid OrderId { get; set; }

        [JsonIgnore]
        public Boolean IsSuccess =>
            StatusCode >= (Int64)HttpStatusCode.OK &&
            StatusCode < (Int64)HttpStatusCode.Ambiguous;

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public Int64 StatusCode { get; set; }
    }
}
