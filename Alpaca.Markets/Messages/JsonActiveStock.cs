namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IActiveStock))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonActiveStock : IActiveStock
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "volume", Required = Required.Always)]
    public Decimal Volume { get; set; }

    [JsonProperty(PropertyName = "trade_count", Required = Required.Always)]
    public UInt64 TradeCount { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IActiveStock)} {{ Symbol = \"{Symbol}\", Volume = {Volume}, TradeCount = {TradeCount} }}";
}
