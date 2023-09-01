namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IMarketMover))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonMarketMover : IMarketMover
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "price", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "change", Required = Required.Always)]
    public Decimal Change { get; set; }

    [JsonProperty(PropertyName = "percent_change", Required = Required.Always)]
    public Decimal PercentChange { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IMarketMover)} {{ Symbol = \"{Symbol}\", Price = {Price}, Change = {Change}, PercentChange = {PercentChange:F2}% }}";
}
