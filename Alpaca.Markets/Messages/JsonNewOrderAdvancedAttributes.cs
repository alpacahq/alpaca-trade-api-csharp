namespace Alpaca.Markets;

internal sealed class JsonNewOrderAdvancedAttributes
{
    [JsonProperty(PropertyName = "limit_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? LimitPrice { get; set; }

    [JsonProperty(PropertyName = "stop_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? StopPrice { get; set; }
}
