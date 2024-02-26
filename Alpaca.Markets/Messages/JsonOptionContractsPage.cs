namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionContractsPage
{
    [JsonProperty(PropertyName = "option_contracts", Required = Required.Always)]
    public List<JsonOptionContract> Contracts { get; set; } = [];
}