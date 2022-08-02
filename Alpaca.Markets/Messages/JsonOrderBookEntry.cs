namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrderBookEntry))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOrderBookEntry : IOrderBookEntry
{
    [JsonProperty(PropertyName = "p", Required = Required.Always)]
    public Decimal Price { get; set; }

    [JsonProperty(PropertyName = "s", Required = Required.Always)]
    public Decimal Size { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{{ Price = {Price}, Size = {Size} }}";
}
