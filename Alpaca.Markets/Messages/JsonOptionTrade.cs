namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ITrade))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionTrade : ITrade, ISymbolMutable
{
    private readonly List<String> _conditionsList = [];

    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String Exchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "p", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "s", Required = Required.Always)]
    public Decimal Size { get; set; }

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonIgnore]
    public TakerSide TakerSide => TakerSide.Unknown;

    [JsonIgnore]
    public String Update => String.Empty;

    [JsonIgnore]
    public String Tape => String.Empty;

    [JsonIgnore]
    public UInt64 TradeId => 0;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions => _conditionsList;
    
    [UsedImplicitly]
    [JsonExtensionData]
    public Dictionary<String, Object> ExtensionData { get; set; } = [];

    [OnDeserialized]
    internal void OnDeserializedMethod(
        StreamingContext context) =>
        _conditionsList.AddRange(ExtensionData.GetConditions());

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
