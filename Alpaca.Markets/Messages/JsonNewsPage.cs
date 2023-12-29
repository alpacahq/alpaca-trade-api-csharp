namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<INewsArticle>) + "<" + nameof(INewsArticle) + ">")]
internal sealed class JsonNewsPage : IPage<INewsArticle>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "news", Required = Required.Default)]
    public List<JsonNewsArticle> ItemsList { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; [ExcludeFromCodeCoverage] set; }

    [JsonIgnore]
    public IReadOnlyList<INewsArticle> Items => ItemsList.EmptyIfNull();

    [JsonIgnore]
    public String Symbol => String.Empty;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
