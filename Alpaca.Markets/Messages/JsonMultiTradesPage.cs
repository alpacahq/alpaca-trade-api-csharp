using System.Runtime.Serialization;

namespace Alpaca.Markets;

internal sealed class JsonMultiTradesPage : IMultiPageMutable<ITrade>
{
    [JsonProperty(PropertyName = "trades", Required = Required.Default)]
    public Dictionary<String, List<JsonHistoricalTrade>?> ItemsDictionary { get; [ExcludeFromCodeCoverage] set; } = new ();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IReadOnlyList<ITrade>> Items { get; set; } =
        new Dictionary<String, IReadOnlyList<ITrade>>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsDictionary.SetSymbol<ITrade, JsonHistoricalTrade>();
}
