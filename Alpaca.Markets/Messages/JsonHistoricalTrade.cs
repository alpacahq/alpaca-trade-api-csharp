namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ITrade))]
internal sealed class JsonHistoricalTrade : ITrade, ISymbolMutable
{
    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String Exchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "p", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "s", Required = Required.Always)]
    public Decimal Size { get; set; }

    [JsonProperty(PropertyName = "i", Required = Required.Default)]
    public UInt64 TradeId { get; set; }

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "u", Required = Required.Default)]
    public String Update { get; set; } = String.Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "c", Required = Required.Default)]
    public List<String> ConditionsList { get; } = new();

    [JsonProperty(PropertyName = "tks", Required = Required.Default)]
    public TakerSide TakerSide { get; [ExcludeFromCodeCoverage] set; } = TakerSide.Unknown;

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions =>
        ConditionsList.EmptyIfNull();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetSymbol(String symbol) => Symbol = symbol;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
