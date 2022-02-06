namespace Alpaca.Markets;

internal sealed class JsonLatestTrade : ITrade
{
    [JsonProperty(PropertyName = "trade", Required = Required.Always)]
    public JsonHistoricalTrade Nested { get; set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public DateTime TimestampUtc => Nested.TimestampUtc;

    [JsonIgnore]
    public UInt64 TradeId => Nested.TradeId;

    [JsonIgnore]
    public String Tape => Nested.Tape;

    [JsonIgnore]
    public String Exchange => Nested.Exchange;

    [JsonIgnore]
    public Decimal Price => Nested.Price;

    [JsonIgnore]
    public Decimal Size => Nested.Size;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions => Nested.Conditions;

    [JsonIgnore]
    public TakerSide TakerSide => Nested.TakerSide;
}
