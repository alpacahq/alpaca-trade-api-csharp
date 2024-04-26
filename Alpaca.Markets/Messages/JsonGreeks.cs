namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IGreeks))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonGreeks : IGreeks
{
    [JsonProperty(PropertyName = "delta", Required = Required.Default)]
    public Decimal? Delta { get; set; }

    [JsonProperty(PropertyName = "gamma", Required = Required.Default)]
    public Decimal? Gamma { get; set; }

    [JsonProperty(PropertyName = "rho", Required = Required.Default)]
    public Decimal? Rho { get; set; }

    [JsonProperty(PropertyName = "theta", Required = Required.Default)]
    public Decimal? Theta { get; set; }

    [JsonProperty(PropertyName = "vega", Required = Required.Default)]
    public Decimal? Vega { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IGreeks)} {{ Delta = {Delta}, Gamma = {Gamma}, Rho = {Rho}, Theta = {Theta}, Vega = {Vega} }}";
}
