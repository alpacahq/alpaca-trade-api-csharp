namespace Alpaca.Markets;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPosition))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonPosition : IPosition
{
    [JsonProperty(PropertyName = "asset_id", Required = Required.Always)]
    public Guid AssetId { get; set; }

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "exchange", Required = Required.Always)]
    public Exchange Exchange { get; set; }

    [JsonProperty(PropertyName = "asset_class", Required = Required.Always)]
    public AssetClass AssetClass { get; set; }

    [JsonProperty(PropertyName = "avg_entry_price", Required = Required.Always)]
    public Decimal AverageEntryPrice { get; set; }

    [JsonProperty(PropertyName = "qty", Required = Required.Always)]
    public Decimal Quantity { get; set; }

    [JsonIgnore]
    public Int64 IntegerQuantity => Quantity.AsInteger();

    [JsonProperty(PropertyName = "qty_available", Required = Required.Default)]
    public Decimal AvailableQuantity { get; set; }

    [JsonIgnore]
    public Int64 IntegerAvailableQuantity => AvailableQuantity.AsInteger();

    [JsonProperty(PropertyName = "side", Required = Required.Default)]
    public PositionSide Side { get; set; }

    [JsonProperty(PropertyName = "market_value", Required = Required.Default)]
    public Decimal? MarketValue { get; set; }

    [JsonProperty(PropertyName = "cost_basis", Required = Required.Always)]
    public Decimal CostBasis { get; set; }

    [JsonProperty(PropertyName = "unrealized_pl", Required = Required.Default)]
    public Decimal? UnrealizedProfitLoss { get; set; }

    [JsonProperty(PropertyName = "unrealized_plpc", Required = Required.Default)]
    public Decimal? UnrealizedProfitLossPercent { get; set; }

    [JsonProperty(PropertyName = "unrealized_intraday_pl", Required = Required.Default)]
    public Decimal? IntradayUnrealizedProfitLoss { get; set; }

    [JsonProperty(PropertyName = "unrealized_intraday_plpc", Required = Required.Default)]
    public Decimal? IntradayUnrealizedProfitLossPercent { get; set; }

    [JsonProperty(PropertyName = "current_price", Required = Required.Default)]
    public Decimal? AssetCurrentPrice { get; set; }

    [JsonProperty(PropertyName = "lastday_price", Required = Required.Default)]
    public Decimal? AssetLastPrice { get; set; }

    [JsonProperty(PropertyName = "change_today", Required = Required.Default)]
    public Decimal? AssetChangePercent { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IPosition)} {{ Symbol = \"{Symbol}\", Side = {Side}, Quantity = {Quantity}, UPL = {UnrealizedProfitLoss} }}";
}
