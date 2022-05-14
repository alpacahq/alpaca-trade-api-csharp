namespace Alpaca.Markets;

internal sealed class JsonAuthRequest
{
    internal sealed class JsonData
    {
        [JsonProperty(PropertyName = "key_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String? KeyId { get; set; }

        [JsonProperty(PropertyName = "secret_key", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String? SecretKey { get; set; }

        [JsonProperty(PropertyName = "oauth_token", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
        public String? OAuthToken { get; set; }
    }

    [JsonProperty(PropertyName = "action", Required = Required.Always)]
    public JsonAction Action { get; set; }

    [JsonProperty(PropertyName = "data", Required = Required.Default)]
    public JsonData? Data { get; set; }
}
