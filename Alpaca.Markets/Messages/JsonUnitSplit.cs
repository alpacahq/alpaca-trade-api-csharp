namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IUnitSplit))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonUnitSplit : IUnitSplit
{
    [JsonProperty(PropertyName = "new_symbol", Required = Required.Always)]
    public String NewSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "old_symbol", Required = Required.Always)]
    public String OldSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "alternate_symbol", Required = Required.Always)]
    public String AlternateSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "new_rate", Required = Required.Always)]
    public Decimal NewRate { get; set; }

    [JsonProperty(PropertyName = "old_rate", Required = Required.Always)]
    public Decimal OldRate { get; set; }

    [JsonProperty(PropertyName = "alternate_rate", Required = Required.Always)]
    public Decimal AlternateRate { get; set; }

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
        $"{nameof(IUnitSplit)} {{ NewSymbol = {NewSymbol}, NewRate = {NewRate}, OldSymbol = {OldSymbol}, OldRate = {OldRate} }}";
}
