namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAuction))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAuction : IAuction, ISymbolMutable 
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "d", Required = Required.Always)]
    internal DateTime DateTime { get; set; }

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "o", Required = Required.Default)]
    internal List<JsonAuctionEntry> OpeningsList { get; set; } = [];

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "c", Required = Required.Default)]
    internal List<JsonAuctionEntry> ClosingsList { get; set; } = [];

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonIgnore]
    public DateOnly Date => DateOnly.FromDateTime(DateTime);

    [JsonIgnore]
    public IReadOnlyList<IAuctionEntry> Openings { get; private set; } = new List<IAuctionEntry>();

    [JsonIgnore]
    public IReadOnlyList<IAuctionEntry> Closings { get; private set; } = new List<IAuctionEntry>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetSymbol(String symbol) => Symbol = symbol;

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        Openings = OpeningsList.EmptyIfNull();
        Closings = ClosingsList.EmptyIfNull();
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IPosition)} {{ Symbol = \"{Symbol}\", Bids.Count = {Openings.Count}, Asks.Count = {Closings.Count} }}";
}
