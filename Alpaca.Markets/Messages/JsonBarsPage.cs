using System.Runtime.Serialization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
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
    internal void OnDeserializedMethod(
        StreamingContext context)
    {
        // ReSharper disable once ConstantConditionalAccessQualifier
        ItemsList?.ForEach(_ => _.SetSymbol(Symbol));
        Items = ItemsList.EmptyIfNull();
    }
}
