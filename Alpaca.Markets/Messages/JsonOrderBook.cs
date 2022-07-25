namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderBook))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOrderBook : JsonRealTimeBase, IOrderBook
{
    [DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderBookEntry))]
    internal sealed class Entry : IOrderBookEntry
    {
        [JsonProperty(PropertyName = "p", Required = Required.Always)]
        public Decimal Price { get; set; }

        [JsonProperty(PropertyName = "s", Required = Required.Always)]
        public Decimal Size { get; set; }

        [ExcludeFromCodeCoverage]
        public override String ToString() =>
            JsonConvert.SerializeObject(this);

        [ExcludeFromCodeCoverage]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String DebuggerDisplay =>
            $"{{ Price = {Price}, Size = {Size} }}";
    }

    [JsonProperty(PropertyName = "x", Required = Required.Always)]
    public String Exchange { get; set; } = String.Empty;

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "b", Required = Required.Always)]
    internal List<Entry> BidsList { get; set; } = new();

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "a", Required = Required.Always)]
    internal List<Entry> AsksList { get; set; } = new();

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
