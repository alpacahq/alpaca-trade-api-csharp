namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IStockAndCashMerger))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonStockAndCashMerger : IStockAndCashMerger
{
    [JsonProperty(PropertyName = "acquirer_symbol", Required = Required.Always)]
    public String AcquirerSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "acquiree_symbol", Required = Required.Always)]
    public String AcquireeSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "acquirer_rate", Required = Required.Always)]
    public Decimal AcquirerRate { get; set; }

    [JsonProperty(PropertyName = "acquiree_rate", Required = Required.Always)]
    public Decimal AcquireeRate { get; set; }

    [JsonProperty(PropertyName = "cash_rate", Required = Required.Always)]
    public Decimal CashRate { get; set; }

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
        $"{nameof(IStockAndCashMerger)} {{ AcquirerSymbol = {AcquirerSymbol}, AcquireeSymbol = {AcquireeSymbol} }}";
}
