namespace Alpaca.Markets;

internal abstract class JsonRealTimeBase
{
    [JsonProperty(PropertyName = "T", Required = Required.Always)]
    public String Channel { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "S", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonIgnore]
    public DateTime TimeUtc => TimestampUtc;
}
