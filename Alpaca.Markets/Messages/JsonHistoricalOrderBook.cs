namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderBook))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonHistoricalOrderBook : IOrderBook, ISymbolMutable 
{
    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "b", Required = Required.Always)]
    internal List<JsonOrderBookEntry> BidsList { get; set; } = [];

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "a", Required = Required.Always)]
    internal List<JsonOrderBookEntry> AsksList { get; set; } = [];

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonIgnore]
    public String Exchange => String.Empty;

    [JsonIgnore]
    public Boolean IsReset => false;

    [JsonIgnore]
    public IReadOnlyList<IOrderBookEntry> Bids { get; private set; } = new List<IOrderBookEntry>();

    [JsonIgnore]
    public IReadOnlyList<IOrderBookEntry> Asks { get; private set; } = new List<IOrderBookEntry>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetSymbol(String symbol) => Symbol = symbol;

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        Bids = BidsList.EmptyIfNull();
        Asks = AsksList.EmptyIfNull();
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IPosition)} {{ Symbol = \"{Symbol}\", Bids.Count = {Bids.Count}, Asks.Count = {Asks.Count} }}";
}
