namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderBook))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonRealTimeOrderBook : JsonRealTimeBase, IOrderBook
{
    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String Exchange { get; set; } = String.Empty;

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "b", Required = Required.Always)]
    internal List<JsonOrderBookEntry> BidsList { get; set; } = [];

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "a", Required = Required.Always)]
    internal List<JsonOrderBookEntry> AsksList { get; set; } = [];

    [JsonProperty(PropertyName = "r", Required = Required.Default)]
    public Boolean IsReset { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IOrderBookEntry> Bids { get; private set; } = new List<IOrderBookEntry>();

    [JsonIgnore]
    public IReadOnlyList<IOrderBookEntry> Asks { get; private set; } = new List<IOrderBookEntry>();

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
