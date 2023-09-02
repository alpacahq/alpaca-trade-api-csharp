namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<ITrade>) + "<" + nameof(IBar) + ">")]
internal sealed class JsonMultiBarsPage : IMultiPageMutable<IBar>
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "bars", Required = Required.Default)]
    public Dictionary<String, List<JsonHistoricalBar>?> ItemsDictionary { get; [ExcludeFromCodeCoverage] set; } = new();

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

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
