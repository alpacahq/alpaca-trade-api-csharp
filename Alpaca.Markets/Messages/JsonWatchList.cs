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
#pragma warning disable CA1825 // Avoid zero-length array allocations.
        private static readonly IReadOnlyList<IAsset> _empty = new IAsset[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.

        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid WatchListId { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "updated_at", Required = Required.Default)]
        public DateTime? Updated { get; set; }

        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "account_id", Required = Required.Always)]
        public Guid AccountId { get; set; }

        [JsonProperty(PropertyName = "assets", Required = Required.Default)]
        public List<JsonAsset> AssetsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IAsset> Assets => AssetsList ?? _empty;
    }
}
