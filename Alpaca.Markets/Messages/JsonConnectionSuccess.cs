namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonConnectionSuccess
{
    [JsonProperty(PropertyName = "msg", Required = Required.Always)]
    public ConnectionStatus Status { get; set; }
}
