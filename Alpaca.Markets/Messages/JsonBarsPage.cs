using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonBarsPage : IPage<IBar>
    {
        [JsonProperty(PropertyName = "bars", Required = Required.Default)]
        public List<JsonHistoricalBar> ItemsList { get; set; } = new ();

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
        public String? NextPageToken { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IBar> Items { get; private set; } = new List<IBar>();
            
        [OnDeserialized]
        internal void OnDeserializedMethod(
            StreamingContext context)
        {
            // ReSharper disable once ConstantConditionalAccessQualifier
            ItemsList?.ForEach(_ => _.Symbol = Symbol);
            Items = ItemsList.EmptyIfNull();
        }
    }
}
