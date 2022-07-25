namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<IQuote>) + "<" + nameof(IQuote) + ">")]
internal sealed class JsonQuotesPage<TQuote> : IPageMutable<IQuote>
    where TQuote : IQuote, ISymbolMutable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
    public List<TQuote> ItemsList { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IQuote> Items { get; set; } = new List<IQuote>();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsList.SetSymbol(Symbol).EmptyIfNull<IQuote, TQuote>();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
