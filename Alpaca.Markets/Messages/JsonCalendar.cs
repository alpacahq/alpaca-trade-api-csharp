using System.Runtime.Serialization;

namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCalendar : ICalendar
{
    [JsonConverter(typeof(AssumeLocalIsoDateConverter))]
    [JsonProperty(PropertyName = "date", Required = Required.Always)]
    public DateTime TradingDateEst { get; set; }

    [JsonConverter(typeof(AssumeLocalIsoTimeConverter))]
    [JsonProperty(PropertyName = "open", Required = Required.Always)]
    public DateTime TradingOpenTimeEst { get; set; }

    [JsonConverter(typeof(AssumeLocalIsoTimeConverter))]
    [JsonProperty(PropertyName = "close", Required = Required.Always)]
    public DateTime TradingCloseTimeEst { get; set; }

    [JsonIgnore]
    public DateTime TradingDateUtc { get; private set; }

    [JsonIgnore]
    public DateOnly TradingDate { get; private set; }

    [JsonIgnore]
    public DateTime TradingOpenTimeUtc { get; private set; }

    [JsonIgnore]
    public TimeOnly OpenTimeEst { get; private set; }

    [JsonIgnore]
    public TimeOnly OpenTimeUtc { get; private set; }

    [JsonIgnore]
    public DateTime TradingCloseTimeUtc { get; private set; }

    [JsonIgnore]
    public TimeOnly CloseTimeEst { get; private set; }

    [JsonIgnore]
    public TimeOnly CloseTimeUtc { get; private set; }

    [OnDeserialized]
    internal void OnDeserializedMethod(
        StreamingContext context)
    {
        TradingDateEst = DateTime.SpecifyKind(
            TradingDateEst.Date, DateTimeKind.Unspecified);

        TradingOpenTimeEst = DateTime.SpecifyKind(
            TradingDateEst.Date.Add(TradingOpenTimeEst.TimeOfDay),
            DateTimeKind.Unspecified);
        TradingCloseTimeEst = DateTime.SpecifyKind(
            TradingDateEst.Date.Add(TradingCloseTimeEst.TimeOfDay),
            DateTimeKind.Unspecified);

        TradingOpenTimeUtc = CustomTimeZone
            .ConvertFromEstToUtc(TradingOpenTimeEst);
        TradingCloseTimeUtc = CustomTimeZone
            .ConvertFromEstToUtc(TradingCloseTimeEst);

        TradingDateUtc = TradingDateEst.AsUtcDateTime();

        TradingDate = DateOnly.FromDateTime(TradingDateUtc);
        OpenTimeEst = TimeOnly.FromDateTime(TradingOpenTimeEst);
        OpenTimeUtc = TimeOnly.FromDateTime(TradingOpenTimeUtc);
        CloseTimeEst = TimeOnly.FromDateTime(TradingCloseTimeEst);
        CloseTimeUtc = TimeOnly.FromDateTime(TradingCloseTimeUtc);
    }
}
