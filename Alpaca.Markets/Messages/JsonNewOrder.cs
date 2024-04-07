namespace Alpaca.Markets;

internal sealed class JsonNewOrder
{
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "qty", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? Quantity { get; set; }

    [JsonProperty(PropertyName = "notional", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? Notional { get; set; }

    [JsonProperty(PropertyName = "side", Required = Required.Always)]
    public OrderSide OrderSide { get; set; }

    [JsonProperty(PropertyName = "type", Required = Required.Always)]
    public OrderType OrderType { get; set; }

    [JsonProperty(PropertyName = "time_in_force", Required = Required.Always)]
    public TimeInForce TimeInForce { get; set; }

    [JsonProperty(PropertyName = "limit_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? LimitPrice { get; set; }

    [JsonProperty(PropertyName = "stop_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? StopPrice { get; set; }

    [JsonProperty(PropertyName = "trail_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? TrailOffsetInDollars { get; set; }

    [JsonProperty(PropertyName = "trail_percent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? TrailOffsetInPercent { get; set; }

    [JsonProperty(PropertyName = "client_order_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public String? ClientOrderId { get; set; }

    [JsonProperty(PropertyName = "extended_hours", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Boolean? ExtendedHours { get; set; }

    [JsonProperty(PropertyName = "order_class", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public OrderClass? OrderClass { get; set; }

    [JsonProperty(PropertyName = "take_profit", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public JsonNewOrderAdvancedAttributes? TakeProfit { get; set; }

    [JsonProperty(PropertyName = "stop_loss", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public JsonNewOrderAdvancedAttributes? StopLoss { get; set; }

    [JsonProperty(PropertyName = "position_intent", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public PositionIntent? PositionIntent { get; set; }
}
