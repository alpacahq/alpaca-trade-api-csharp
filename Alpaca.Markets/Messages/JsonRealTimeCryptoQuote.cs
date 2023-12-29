namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IQuote))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonRealTimeCryptoQuote : JsonRealTimeBase, IQuote
{
    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String BidExchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "bp", Required = Required.Always)]
    public Decimal BidPrice { get; set; }

    [JsonProperty(PropertyName = "ap", Required = Required.Always)]
    public Decimal AskPrice { get; set; }

    [JsonProperty(PropertyName = "bs", Required = Required.Always)]
    public Decimal BidSize { get; set; }

    [JsonProperty(PropertyName = "as", Required = Required.Always)]
    public Decimal AskSize { get; set; }

    [JsonProperty(PropertyName = "c", Required = Required.Default)]
    public List<String> ConditionsList { get; } = [];

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonIgnore]
    public String AskExchange => BidExchange;

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
