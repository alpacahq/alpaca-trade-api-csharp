namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ISpinOff))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonSpinOff : ISpinOff
{
    [JsonProperty(PropertyName = "source_symbol", Required = Required.Always)]
    public String SourceSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "new_symbol", Required = Required.Always)]
    public String NewSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "source_rate", Required = Required.Always)]
    public Decimal SourceRate { get; set; }

    [JsonProperty(PropertyName = "new_rate", Required = Required.Always)]
    public Decimal NewRate { get; set; }

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
    [JsonProperty(PropertyName = "due_bill_redemption_date", Required = Required.Default)]
    public DateOnly? DueBillRedemptionDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(ISpinOff)} {{ SourceSymbol = {SourceSymbol}, SourceRate = {SourceRate}, NewSymbol = {NewSymbol}, NewRate = {NewRate} }}";
}
