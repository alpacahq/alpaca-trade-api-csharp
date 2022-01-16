namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonNewsPage : IPage<INewsArticle>
{
    [JsonProperty(PropertyName = "news", Required = Required.Default)]
    public List<JsonNewsArticle> ItemsList { get; set; } = new ();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyList<INewsArticle> Items => ItemsList.EmptyIfNull();

    [JsonIgnore]
    public String Symbol => String.Empty;
}