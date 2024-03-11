namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IQuote))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionQuote : IQuote, ISymbolMutable
{
    private readonly List<String> _conditionsList = [];

    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "ax", Required = Required.Always)]
    public String AskExchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "ap", Required = Required.Default)]
    public Decimal AskPrice { get; set; }

    [JsonProperty(PropertyName = "as", Required = Required.Default)]
    public Decimal AskSize { get; set; }

    [JsonProperty(PropertyName = "bx", Required = Required.Always)]
    public String BidExchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "bp", Required = Required.Default)]
    public Decimal BidPrice { get; set; }

    [JsonProperty(PropertyName = "bs", Required = Required.Default)]
    public Decimal BidSize { get; set; }

    [JsonIgnore]
    public String Tape => String.Empty;

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

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
