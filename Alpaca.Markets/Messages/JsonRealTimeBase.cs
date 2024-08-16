using MessagePack;

namespace Alpaca.Markets;

[MessagePackObject]
public abstract class JsonRealTimeBase
{
    [Key("T")]
    [JsonProperty(PropertyName = "T", Required = Required.Always)]
    public String Channel { get; set; } = String.Empty;

    [Key("S")]
    [JsonProperty(PropertyName = "S", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [Key("t")]
    [JsonProperty(PropertyName = "t", Required = Required.Always)]
    public DateTime TimestampUtc { get; set; }

    [JsonIgnore]
    public DateTime TimeUtc => TimestampUtc;
}
