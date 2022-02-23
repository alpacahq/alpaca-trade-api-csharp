using System.Runtime.Serialization;

namespace Alpaca.Markets;

#pragma warning disable CS0618
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCalendar : ICalendar, IIntervalCalendar
#pragma warning restore CS0618
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

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingDateEst =>
        this.GetTradingDateFast().ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingDateUtc =>
        this.GetTradingDateFast().ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingOpenTimeEst => this.GetTradingOpenTimeEstFast();

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingCloseTimeEst => this.GetTradingCloseTimeEstFast();

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingOpenTimeUtc => this.GetTradingOpenTimeUtcFast();

    [JsonIgnore]
    [ExcludeFromCodeCoverage]
    public DateTime TradingCloseTimeUtc => this.GetTradingCloseTimeUtcFast();

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        Trading = new OpenClose(TradingDate, TradingOpen, TradingClose);
        Session = new OpenClose(TradingDate, SessionOpen, SessionClose);
    }
}
