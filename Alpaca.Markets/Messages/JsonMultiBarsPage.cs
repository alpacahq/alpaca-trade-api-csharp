using System.Runtime.Serialization;

namespace Alpaca.Markets;

internal sealed class JsonMultiBarsPage : IMultiPageMutable<IBar>
{
    [JsonProperty(PropertyName = "bars", Required = Required.Default)]
    public Dictionary<String, List<JsonHistoricalBar>?> ItemsDictionary { get; [ExcludeFromCodeCoverage] set; } = new ();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IReadOnlyList<IBar>> Items { get; set; } =
        new Dictionary<String, IReadOnlyList<IBar>>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsDictionary.SetSymbol<IBar, JsonHistoricalBar>();
}
