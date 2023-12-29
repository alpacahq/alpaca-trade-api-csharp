namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ITrade))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonRealTimeTrade : JsonRealTimeBase, ITrade
{
    [JsonProperty(PropertyName = "i", Required = Required.Default)]
    public UInt64 TradeId { get; set; }

    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String Exchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "p", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "s", Required = Required.Always)]
    public Decimal Size { get; set; }

    [JsonProperty(PropertyName = "c", Required = Required.Default)]
    public List<String> ConditionsList { get; set; } = [];

    [JsonProperty(PropertyName = "tks", Required = Required.Default)]
    public TakerSide TakerSide { get; set; } = TakerSide.Unknown;

    [JsonProperty(PropertyName = "u", Required = Required.Default)]
    public String Update { get; set; } = String.Empty;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions =>
        ConditionsList.EmptyIfNull();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
