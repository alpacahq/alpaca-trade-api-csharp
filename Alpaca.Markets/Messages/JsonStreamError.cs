namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonStreamError
{
    [JsonProperty(PropertyName = "code", Required = Required.Default)]
    public Int32 Code { get; set; }

    [JsonProperty(PropertyName = "msg", Required = Required.Default)]
    public String Message { get; set; } = String.Empty;
}
