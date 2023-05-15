namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAccount))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCalendar : IIntervalCalendar
{
    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "date", Required = Required.Always)]
    public DateOnly TradingDate { get; set; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    [JsonProperty(PropertyName = "open", Required = Required.Always)]
    public TimeOnly TradingOpen { get; set; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    [JsonProperty(PropertyName = "close", Required = Required.Always)]
    public TimeOnly TradingClose { get; set; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    [JsonProperty(PropertyName = "session_open", Required = Required.Always)]
    public TimeOnly SessionOpen { get; set; }

    [JsonConverter(typeof(TimeOnlyConverter))]
    [JsonProperty(PropertyName = "session_close", Required = Required.Always)]
    public TimeOnly SessionClose { get; set; }

    [JsonIgnore]
    public OpenClose Trading { get; private set; }

    [JsonIgnore]
    public OpenClose Session { get; private set; }

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        Trading = new OpenClose(TradingDate, TradingOpen, TradingClose);
        Session = new OpenClose(TradingDate, SessionOpen, SessionClose);
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IIntervalCalendar)} {{ Date = {TradingDate}, Trading (EST) {{ {TradingOpen} - {TradingClose} }}, Session (EST) {{ {SessionOpen} - {SessionClose} }} }}";
}
