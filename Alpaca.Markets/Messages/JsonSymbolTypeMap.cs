using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonSymbolTypeMap
    {
        internal sealed class JsonResults
        {
            [JsonProperty(PropertyName = "types", Required = Required.Default)]
            public Dictionary<String, String> StockTypes { get; set; } = new Dictionary<String, String>();

            [JsonProperty(PropertyName = "indexTypes", Required = Required.Default)]
            public Dictionary<String, String> IndexTypes { get; set; } = new Dictionary<String, String>();
        }

        [JsonProperty(PropertyName = "status", Required = Required.Default)]
        public String Status { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "results", Required = Required.Default)]
        public JsonResults Results { get; set; } = new JsonResults();
    }
}
