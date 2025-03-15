namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ICashDividend))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCashDividend : ICashDividend
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "rate", Required = Required.Always)]
    public Decimal Rate { get; set; }

    [JsonProperty(PropertyName = "foreign", Required = Required.Always)]
    public Boolean IsForeign { get; set; }

    [JsonProperty(PropertyName = "special", Required = Required.Always)]
    public Boolean IsSpecial { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "process_date", Required = Required.Always)]
    public DateOnly ProcessDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "ex_date", Required = Required.Always)]
    public DateOnly ExecutionDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "record_date", Required = Required.Default)]
    public DateOnly? RecordDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "payable_date", Required = Required.Default)]
    public DateOnly? PayableDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "due_bill_off_date", Required = Required.Default)]
    public DateOnly? DueBillOffDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "due_bill_on_date", Required = Required.Default)]
    public DateOnly? DueBillOnDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(ICashDividend)} {{ Symbol = {Symbol}, Rate = {Rate} }}";
}
