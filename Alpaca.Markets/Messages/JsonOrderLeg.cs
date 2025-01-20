namespace Alpaca.Markets;

internal sealed class JsonOrderLeg
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "ratio_qty", Required = Required.Always)]
    public Decimal RatioQuantity { get; set; }

    [JsonProperty(PropertyName = "side", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public OrderSide? OrderSide { get; set; }

    [JsonProperty(PropertyName = "position_intent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public PositionIntent? PositionIntent { get; set; }
}
