namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPositionActionStatus))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonPositionActionStatus : IPositionActionStatus
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public Boolean IsSuccess => StatusCode.IsSuccessHttpStatusCode();

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public Int64 StatusCode { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IPositionActionStatus)} {{ Symbol = \"{Symbol}\", StatusCode = {StatusCode}, IsSuccess = {IsSuccess} }}";
}
