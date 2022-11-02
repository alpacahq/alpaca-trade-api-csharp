namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAuctionEntry))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAuctionEntry : IAuctionEntry
{
    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "p", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "s", Required = Required.Always)]
    public Decimal Size { get; set; }

    [JsonProperty(PropertyName = "x", Required = Required.Default)]
    public String Exchange { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "c", Required = Required.Always)]
    public String Condition { get; set; } = String.Empty;

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{{ Price = {Price}, Size = {Size} }}";
}
