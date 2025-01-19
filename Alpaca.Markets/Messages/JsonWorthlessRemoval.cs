namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IWorthlessRemoval))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonWorthlessRemoval : IWorthlessRemoval
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "process_date", Required = Required.Always)]
    public DateOnly ProcessDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IWorthlessRemoval)} {{ Symbol = {Symbol}, ProcessDate = {ProcessDate} }}";
}
