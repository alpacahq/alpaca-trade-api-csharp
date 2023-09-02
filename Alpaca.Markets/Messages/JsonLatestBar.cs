namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IBar))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonLatestBar : IBar
{
    [JsonProperty(PropertyName = "bar", Required = Required.Always)]
    public JsonHistoricalBar Nested { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public DateTime TimeUtc => Nested.TimeUtc;

    [JsonIgnore]
    public Decimal Open => Nested.Open;

    [JsonIgnore]
    public Decimal High => Nested.High;

    [JsonIgnore]
    public Decimal Low => Nested.Low;

    [JsonIgnore]
    public Decimal Close => Nested.Close;

    [JsonIgnore]
    public Decimal Volume => Nested.Volume;

    [JsonIgnore]
    public Decimal Vwap => Nested.Vwap;

    [JsonIgnore]
    public UInt64 TradeCount => Nested.TradeCount;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        this.ToDebuggerDisplayString();
}
