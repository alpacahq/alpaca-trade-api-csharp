namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderImbalance))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOrderImbalance : JsonRealTimeBase, IOrderImbalance
{
    [JsonProperty(PropertyName = "p", Required = Required.Default)]
    public Decimal ReferencePrice { get; set; }

    [JsonProperty(PropertyName = "ps", Required = Required.Default)]
    public Int64 PairedShares { get; set; }

    [JsonProperty(PropertyName = "is", Required = Required.Default)]
    public Int64 ImbalanceShares { get; set; }

    [JsonProperty(PropertyName = "side", Required = Required.Default)]
    public String ImbalanceSide { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "at", Required = Required.Default)]
    public String AuctionType { get; set; } = String.Empty;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IOrderImbalance)} {{ Symbol = \"{Symbol}\", ReferencePrice = {ReferencePrice}, ImbalanceShares = {ImbalanceShares}, ImbalanceSide = \"{ImbalanceSide}\", AuctionType = \"{AuctionType}\" }}";
}