namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IMarketMovers))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonMarketMovers : IMarketMovers
{
    [JsonProperty(PropertyName = "losers", Required = Required.Always)]
    public List<JsonMarketMover> LosersList { get; [ExcludeFromCodeCoverage] set; } = [];

    [JsonProperty(PropertyName = "gainers", Required = Required.Always)]
    public List<JsonMarketMover> GainersList { get; [ExcludeFromCodeCoverage] set; } = [];

    public IReadOnlyList<IMarketMover> Losers =>
        LosersList.EmptyIfNull<IMarketMover>();

    public IReadOnlyList<IMarketMover> Gainers =>
        GainersList.EmptyIfNull<IMarketMover>();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IMarketMovers)} {{ Losers.Count = {Losers.Count}, Gainers.Count = {Gainers.Count} }}";
}
