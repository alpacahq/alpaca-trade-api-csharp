namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ISnapshot))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionSnapshot : IOptionSnapshot, ISymbolMutable
{
    [JsonProperty(PropertyName = "latestQuote", Required = Required.Default)]
    public JsonOptionQuote? JsonQuote { get; set; }

    [JsonProperty(PropertyName = "latestTrade", Required = Required.Default)]
    public JsonOptionTrade? JsonTrade { get; set; }

    [JsonProperty(PropertyName = "greeks", Required = Required.Default)]
    public JsonGreeks? JsonGreeks { get; set; }

    [JsonProperty(PropertyName = "impliedVolatility", Required = Required.Default)]
    public Decimal? ImpliedVolatility { get; set; }

    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonIgnore]
    public IQuote? Quote => JsonQuote;

    [JsonIgnore]
    public ITrade? Trade => JsonTrade;

    [JsonIgnore]
    public IGreeks? Greeks => JsonGreeks;

    public void SetSymbol(
        String symbol)
    {
        Symbol = symbol;
        JsonTrade?.SetSymbol(Symbol);
        JsonQuote?.SetSymbol(Symbol);
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
