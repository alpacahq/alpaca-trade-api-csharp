using System.Runtime.Serialization;

namespace Alpaca.Markets;

internal sealed class JsonTradesPage : IPageMutable<ITrade>
{
    [JsonProperty(PropertyName = "trades", Required = Required.Default)]
    public List<JsonHistoricalTrade> ItemsList { get; [ExcludeFromCodeCoverage] set; } = new ();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyList<ITrade> Items { get; set; } = new List<ITrade>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsList.SetSymbol(Symbol).EmptyIfNull();
}
