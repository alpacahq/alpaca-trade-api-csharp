namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonLatestData<TQuote, TTrade, TSnapshot>
{
    [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
    public Dictionary<String, TQuote> Quotes { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "bars", Required = Required.Default)]
    public Dictionary<String, JsonHistoricalBar> Bars { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "trades", Required = Required.Default)]
    public Dictionary<String, TTrade> Trades { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "snapshots", Required = Required.Default)]
    public Dictionary<String, TSnapshot> Snapshots { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "orderbooks", Required = Required.Default)]
    public Dictionary<String, JsonHistoricalOrderBook> OrderBooks { get; [ExcludeFromCodeCoverage] set; } = new();
}
