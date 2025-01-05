namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IRightsDistribution))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonRightsDistribution : IRightsDistribution
{
    [JsonProperty(PropertyName = "source_symbol", Required = Required.Always)]
    public String SourceSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "new_symbol", Required = Required.Always)]
    public String NewSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "rate", Required = Required.Always)]
    public Decimal Rate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "process_date", Required = Required.Always)]
    public DateOnly ProcessDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "ex_date", Required = Required.Always)]
    public DateOnly ExecutionDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "payable_date", Required = Required.Default)]
    public DateOnly? PayableDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "record_date", Required = Required.Default)]
    public DateOnly? RecordDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "expiration_date", Required = Required.Default)]
    public DateOnly? ExpirationDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IRightsDistribution)} {{ SourceSymbol = {SourceSymbol}, NewSymbol = {NewSymbol}, Rate = {Rate} }}";
}
