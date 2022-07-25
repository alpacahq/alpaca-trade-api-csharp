namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<ITrade>) + "<" + nameof(IQuote) + ">")]
internal sealed class JsonMultiQuotesPage<TQuote> : IMultiPageMutable<IQuote>
    where TQuote : IQuote, ISymbolMutable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
    public Dictionary<String, List<TQuote>?> ItemsDictionary { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyDictionary<String, IReadOnlyList<IQuote>> Items { get; set; } =
        new Dictionary<String, IReadOnlyList<IQuote>>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsDictionary.SetSymbol<IQuote, TQuote>();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
