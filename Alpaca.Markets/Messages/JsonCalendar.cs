using System.Runtime.Serialization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
#pragma warning disable CS0618
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
    public Interval<DateTime> TradingOpenCloseUtc { get; private set; }

    [JsonIgnore]
    public Interval<DateTime> SessionOpenCloseUtc { get; private set; }

    [JsonIgnore]
    public Interval<DateTimeOffset> TradingOpenCloseEst { get; private set; }

    [JsonIgnore]
    public Interval<DateTimeOffset> SessionOpenCloseEst { get; private set; }

    [JsonIgnore]
    public DateTime TradingDateEst =>
        this.GetTradingDateFast().ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);

    [JsonIgnore]
    public DateTime TradingDateUtc =>
        this.GetTradingDateFast().ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

    [JsonIgnore]
    public DateTime TradingOpenTimeEst => this.GetTradingOpenTimeEstFast();

    [JsonIgnore]
    public DateTime TradingCloseTimeEst => this.GetTradingCloseTimeEstFast();

    [JsonIgnore]
    public DateTime TradingOpenTimeUtc => this.GetTradingOpenTimeUtcFast();

    [JsonIgnore]
    public DateTime TradingCloseTimeUtc => this.GetTradingCloseTimeUtcFast();

    [OnDeserialized]
    internal void OnDeserializedMethod(
        StreamingContext context)
    {
        TradingOpenCloseEst = new Interval<DateTimeOffset>(
            CustomTimeZone.AsDateTimeOffset(TradingDate, TradingOpen),
            CustomTimeZone.AsDateTimeOffset(TradingDate, TradingClose));
        TradingOpenCloseUtc = asDateTimeInterval(TradingOpenCloseEst);

        SessionOpenCloseEst = new Interval<DateTimeOffset>(
            CustomTimeZone.AsDateTimeOffset(TradingDate, SessionOpen),
            CustomTimeZone.AsDateTimeOffset(TradingDate, SessionClose));
        SessionOpenCloseUtc = asDateTimeInterval(SessionOpenCloseEst);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Interval<DateTime> asDateTimeInterval(
        in Interval<DateTimeOffset> interval) =>
        new (interval.From?.UtcDateTime, interval.Into?.UtcDateTime);
}
