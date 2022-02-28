namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonLatestData
{
    [JsonProperty(PropertyName = "bars", Required = Required.Default)]
    public Dictionary<String, JsonHistoricalBar> Bars { get; [ExcludeFromCodeCoverage] set; } = new ();

    [JsonProperty(PropertyName = "quotes", Required = Required.Default)]
    public Dictionary<String, JsonHistoricalQuote> Quotes { get; [ExcludeFromCodeCoverage] set; } = new ();

    [JsonProperty(PropertyName = "trades", Required = Required.Default)]
    public Dictionary<String, JsonHistoricalTrade> Trades { get; [ExcludeFromCodeCoverage] set; } = new ();
}
