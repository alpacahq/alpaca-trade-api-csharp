namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOrder))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOrder : IOrder
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid OrderId { get; set; }

    [JsonProperty(PropertyName = "client_order_id", Required = Required.Default)]
    public String? ClientOrderId { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "created_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? CreatedAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "updated_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? UpdatedAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "submitted_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? SubmittedAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "filled_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? FilledAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "expired_at ", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? ExpiredAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "canceled_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? CancelledAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "failed_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? FailedAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "replaced_at", Required = Required.Default)]
    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    public DateTime? ReplacedAtUtc { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "asset_id", Required = Required.Always)]
    public Guid AssetId { get; set; }

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "asset_class", Required = Required.Always)]
    public AssetClass AssetClass { get; set; }

    [JsonProperty(PropertyName = "notional", Required = Required.Default)]
    public Decimal? Notional { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "qty", Required = Required.Default)]
    public Decimal? Quantity { get; set; }

    [JsonProperty(PropertyName = "filled_qty", Required = Required.Always)]
    public Decimal FilledQuantity { get; set; }

    [JsonIgnore]
    public Int64 IntegerQuantity => Quantity?.AsInteger() ?? 0L;

    [JsonIgnore]
    public Int64 IntegerFilledQuantity => FilledQuantity.AsInteger();

    [JsonProperty(PropertyName = "type", Required = Required.Always)]
    public OrderType OrderType { get; set; }

    [JsonProperty(PropertyName = "order_class", Required = Required.Always)]
    public OrderClass OrderClass { get; }

    [JsonProperty(PropertyName = "side", Required = Required.Always)]
    public OrderSide OrderSide { get; set; }

    [JsonProperty(PropertyName = "time_in_force", Required = Required.Always)]
    public TimeInForce TimeInForce { get; set; }

    [JsonProperty(PropertyName = "limit_price", Required = Required.Default)]
    public Decimal? LimitPrice { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "stop_price", Required = Required.Default)]
    public Decimal? StopPrice { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "trail_price", Required = Required.Default)]
    public Decimal? TrailOffsetInDollars { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "trail_percent", Required = Required.Default)]
    public Decimal? TrailOffsetInPercent { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "hwm", Required = Required.Default)]
    public Decimal? HighWaterMark { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "filled_avg_price", Required = Required.Default)]
    public Decimal? AverageFillPrice { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public OrderStatus OrderStatus { get; set; }

    [JsonProperty(PropertyName = "replaced_by", Required = Required.Default)]
    public Guid? ReplacedByOrderId { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "replaces", Required = Required.Default)]
    public Guid? ReplacesOrderId { get; [ExcludeFromCodeCoverage] set; }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JsonProperty(PropertyName = "legs", Required = Required.Default)]
    public List<JsonOrder>? LegsList { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IOrder> Legs => LegsList.EmptyIfNull<IOrder, JsonOrder>();

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IOrder)} {{ ID = {OrderId:B}, Symbol = \"{Symbol}\", Side = {OrderSide}, Status = {OrderStatus}, Type = {OrderType} }}";
}
