namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonActiveStocks
{
    [JsonProperty(PropertyName = "most_actives", Required = Required.Always)]
    public List<JsonActiveStock> MostActives { get; } = [];
}
