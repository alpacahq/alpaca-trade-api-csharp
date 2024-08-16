using MessagePack;

namespace Alpaca.Markets;

[MessagePackObject]
public sealed class JsonAuthentication
{
    [Key("action")]
    [JsonProperty(PropertyName = "action", Required = Required.Always)]
    public JsonAction Action { get; set; }

    [Key("key")]
    [JsonProperty(PropertyName = "key", Required = Required.Always)]
    public String KeyId { get; set; } = String.Empty;

    [Key("secret")]
    [JsonProperty(PropertyName = "secret", Required = Required.Always)]
    public String SecretKey { get; set; } = String.Empty;
}
