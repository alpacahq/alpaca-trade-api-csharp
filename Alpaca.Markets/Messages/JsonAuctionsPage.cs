namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPage<IAuction>) + "<" + nameof(IAuction) + ">")]
internal sealed class JsonAuctionsPage : IPageMutable<IAuction>, ISymbolMutable
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "auctions", Required = Required.Default)]
    public List<JsonAuction> ItemsList { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IAuction> Items { get; set; } = new List<IAuction>();

    public void SetSymbol(String symbol) => Symbol = symbol;

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        Items = ItemsList.SetSymbol(Symbol).EmptyIfNull();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
