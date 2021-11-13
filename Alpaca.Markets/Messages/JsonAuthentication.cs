namespace Alpaca.Markets;

internal sealed class JsonAuthentication
{
    [JsonProperty(PropertyName = "action", Required = Required.Always)]
    public JsonAction Action { get; set; }

    [JsonProperty(PropertyName = "key", Required = Required.Always)]
    public String KeyId { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "secret", Required = Required.Always)]
    public String SecretKey { get; set; } = String.Empty;
}
