namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ICorrection))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCorrection : JsonRealTimeBase, ICorrection, ITrade
{
    [JsonProperty(PropertyName = "x", Required = Required.Always)]
    public String Exchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "u", Required = Required.Default)]
    public String Update { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "oi", Required = Required.Default)]
    public UInt64 TradeId { get; set; }

    [JsonProperty(PropertyName = "op", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "os", Required = Required.Always)]
    public Decimal Size { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "oc", Required = Required.Default)]
    public List<String> ConditionsList { get; } = [];

    [JsonProperty(PropertyName = "ci", Required = Required.Default)]
    public UInt64 CorrectedTradeId { get; set; }

    [JsonProperty(PropertyName = "cp", Required = Required.Always)]
    public Decimal CorrectedPrice { get; set; }

    [JsonProperty(PropertyName = "cs", Required = Required.Always)]
    public Decimal CorrectedSize { get; set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "cc", Required = Required.Default)]
    public List<String> CorrectedConditionsList { get; } = [];

    [JsonProperty(PropertyName = "tks", Required = Required.Default)]
    public TakerSide TakerSide { get; set; } = TakerSide.Unknown;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions =>
        ConditionsList.EmptyIfNull();

    [JsonIgnore]
    public ITrade OriginalTrade => this;

    [JsonIgnore]
    public ITrade CorrectedTrade { get; private set; } = new JsonRealTimeTrade();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _) =>
        CorrectedTrade = new JsonRealTimeTrade
        {
            Tape = Tape,
            Symbol = Symbol,
            Update = Update,
            Channel = Channel,
            Exchange = Exchange,
            TimestampUtc = TimestampUtc,
            Size = CorrectedSize,
            Price = CorrectedPrice,
            TradeId = CorrectedTradeId,
            ConditionsList = CorrectedConditionsList
        };

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(ICorrection)} {{ {OriginalTrade.ToDebuggerDisplayString()} => {CorrectedTrade.ToDebuggerDisplayString()} }}";
}
