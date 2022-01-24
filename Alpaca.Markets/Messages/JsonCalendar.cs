using System.Runtime.Serialization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCalendar : ICalendar, IIntervalCalendar
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
        TradingDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);

    [JsonIgnore]
    public DateTime TradingDateUtc =>
        TradingDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

    [JsonIgnore] public DateTime TradingOpenTimeEst =>
        TradingOpenCloseEst.From!.Value.DateTime;

    [JsonIgnore]
    public DateTime TradingCloseTimeEst =>
        TradingOpenCloseEst.Into!.Value.DateTime;

    [JsonIgnore]
    public DateTime TradingOpenTimeUtc =>
        TradingOpenCloseEst.From!.Value.UtcDateTime;

    [JsonIgnore]
    public DateTime TradingCloseTimeUtc =>
        TradingOpenCloseEst.Into!.Value.UtcDateTime;

    [JsonIgnore]
    public TimeOnly OpenTimeEst => TimeOnly.FromDateTime(TradingOpenTimeEst);

    [JsonIgnore]
    public TimeOnly OpenTimeUtc => TimeOnly.FromDateTime(TradingOpenTimeUtc);

    [JsonIgnore]
    public TimeOnly CloseTimeEst => TimeOnly.FromDateTime(TradingCloseTimeEst);

    [JsonIgnore]
    public TimeOnly CloseTimeUtc => TimeOnly.FromDateTime(TradingCloseTimeUtc);

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
