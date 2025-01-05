namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ICashMerger))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal class JsonCashMerger : ICashMerger
{
    [JsonProperty(PropertyName = "acquirer_symbol", Required = Required.Default)]
    public String? AcquirerSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "acquiree_symbol", Required = Required.Always)]
    public String AcquireeSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "rate", Required = Required.Always)]
    public Decimal Rate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "process_date", Required = Required.Always)]
    public DateOnly ProcessDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "effective_date", Required = Required.Always)]
    public DateOnly EffectiveDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "payable_date", Required = Required.Default)]
    public DateOnly? PayableDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(ICashMerger)} {{ AcquirerSymbol = {AcquirerSymbol}, AcquireeSymbol = {AcquireeSymbol} }}";
}
