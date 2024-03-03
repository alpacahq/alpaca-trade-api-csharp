namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<ITrade>) + "<" + nameof(ITrade) + ">")]
internal sealed class JsonMultiTradesPage<TTrade> : IMultiPageMutable<ITrade>
    where TTrade : ITrade, ISymbolMutable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "trades", Required = Required.Default)]
    public Dictionary<String, List<TTrade>?> ItemsDictionary { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IReadOnlyList<ITrade>> Items { get; set; } =
        new Dictionary<String, IReadOnlyList<ITrade>>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsDictionary.SetSymbol<ITrade, TTrade>();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
