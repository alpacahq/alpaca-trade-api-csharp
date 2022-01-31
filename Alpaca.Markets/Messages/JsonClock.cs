namespace Alpaca.Markets;

internal sealed class JsonClock : IClock
{
    [JsonProperty(PropertyName = "timestamp", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime TimestampUtc { get; set; }

    [JsonProperty(PropertyName = "is_open", Required = Required.Always)]
    public Boolean IsOpen { get; set; }

    [JsonProperty(PropertyName = "next_open", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime NextOpenUtc { get; set; }

    [JsonProperty(PropertyName = "next_close", Required = Required.Always)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime NextCloseUtc { get; set; }
}
