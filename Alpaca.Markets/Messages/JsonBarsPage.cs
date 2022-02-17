using System.Runtime.Serialization;

namespace Alpaca.Markets;

internal sealed class JsonBarsPage : IPageMutable<IBar>
{
    [JsonProperty(PropertyName = "bars", Required = Required.Default)]
    public List<JsonHistoricalBar> ItemsList { get; set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IBar> Items { get; set; } = new List<IBar>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsList.SetSymbol(Symbol).EmptyIfNull();
}
