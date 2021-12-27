namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonTradingStatus : JsonRealTimeBase, IStatus
{
    [JsonProperty(PropertyName = "sc", Required = Required.Default)]
    public String StatusCode { get; } = String.Empty;

    [JsonProperty(PropertyName = "sm", Required = Required.Default)]
    public String StatusMessage { get; } = String.Empty;

    [JsonProperty(PropertyName = "rc", Required = Required.Default)]
    public String ReasonCode { get; } = String.Empty;

    [JsonProperty(PropertyName = "rm", Required = Required.Default)]
    public String ReasonMessage { get; } = String.Empty;

    [JsonProperty(PropertyName = "z", Required = Required.Default)]
    public String Tape { get; } = String.Empty;
}
