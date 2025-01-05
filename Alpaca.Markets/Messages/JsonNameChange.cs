namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(INameChange))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonNameChange : INameChange
{
    [JsonProperty(PropertyName = "old_symbol", Required = Required.Always)]
    public String OldSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "new_symbol", Required = Required.Always)]
    public String NewSymbol { get; set; } = String.Empty;

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "process_date", Required = Required.Always)]
    public DateOnly ProcessDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(INameChange)} {{ OldSymbol = {OldSymbol}, NewSymbol = {NewSymbol} }}";
}
