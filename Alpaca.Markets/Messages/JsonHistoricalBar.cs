namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IBar))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonHistoricalBar : IBar, ISymbolMutable
{
    [JsonIgnore]
    public String Symbol { get; private set; } = String.Empty;

    [JsonProperty(PropertyName = "o", Required = Required.Always)]
    public Decimal Open { get; set; }

    [JsonProperty(PropertyName = "c", Required = Required.Always)]
    public Decimal Close { get; set; }

    [JsonProperty(PropertyName = "l", Required = Required.Always)]
    public Decimal Low { get; set; }

    [JsonProperty(PropertyName = "h", Required = Required.Always)]
    public Decimal High { get; set; }

    [JsonProperty(PropertyName = "v", Required = Required.Always)]
    public Decimal Volume { get; set; }

    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimeUtc { get; set; }

    [JsonProperty(PropertyName = "vw", Required = Required.Default)]
    public Decimal Vwap { get; set; }

    [JsonProperty(PropertyName = "n", Required = Required.Default)]
    public UInt64 TradeCount { get; set; }

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
