namespace Alpaca.Markets;

internal sealed class JsonLatestQuote<TQuote> : IQuote
    where TQuote : IQuote, new()
{
    [JsonProperty(PropertyName = "quote", Required = Required.Always)]
    public TQuote Nested { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonIgnore]
    public DateTime TimestampUtc => Nested.TimestampUtc;

    [JsonIgnore]
    public String AskExchange => Nested.AskExchange;

    [JsonIgnore]
    public String BidExchange => Nested.BidExchange;

    [JsonIgnore]
    public Decimal BidPrice => Nested.BidPrice;

    [JsonIgnore]
    public Decimal AskPrice => Nested.AskPrice;

    [JsonIgnore]
    public Decimal BidSize => Nested.BidSize;

    [JsonIgnore]
    public Decimal AskSize => Nested.AskSize;

    [JsonIgnore]
    public String Tape => Nested.Tape;

    [JsonIgnore]
    public IReadOnlyList<String> Conditions => Nested.Conditions;
}
