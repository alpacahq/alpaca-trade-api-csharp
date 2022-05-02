namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IQuote))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonLatestQuote<TQuote> : IQuote
    where TQuote : IQuote, new()
{
    [JsonProperty(PropertyName = "quote", Required = Required.Always)]
    public TQuote Nested { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public DateTime TimestampUtc => Nested.TimestampUtc;

    [JsonIgnore]
    public String AskExchange => Nested.AskExchange;

    [JsonIgnore]
    public String BidExchange => Nested.BidExchange;

    [JsonIgnore]
    public Decimal BidPrice => Nested.BidPrice;

    [JsonIgnore]
    public Decimal AskPrice => Nested.AskPrice;

    [JsonIgnore]
    public Decimal BidSize => Nested.BidSize;

    [JsonIgnore]
    public Decimal AskSize => Nested.AskSize;

    [JsonIgnore]
    public String Tape => Nested.Tape;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions => Nested.Conditions;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
