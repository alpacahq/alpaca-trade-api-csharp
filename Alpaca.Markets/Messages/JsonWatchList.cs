using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonWatchList : IWatchList
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid WatchListId { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime CreatedUtc  { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Default)]
        [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
        public DateTime? UpdatedUtc { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "account_id", Required = Required.Always)]
        public Guid AccountId { get; set; }

        [JsonProperty(PropertyName = "assets", Required = Required.Default)]
        public List<JsonAsset>? AssetsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IAsset> Assets => AssetsList.EmptyIfNull();
    }
}
