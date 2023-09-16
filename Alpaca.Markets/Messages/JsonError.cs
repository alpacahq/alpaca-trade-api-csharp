namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonError : IErrorInformation
{
    [JsonProperty(PropertyName = "code", Required = Required.Default)]
    public Int32? Code { get; set; }

    [JsonProperty(PropertyName = "message", Required = Required.Default)]
    public String Message { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "symbol", Required = Required.Default)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "open_orders", Required = Required.Default)]
    public Int32? OpenOrdersCount { get; set; }

    [JsonProperty(PropertyName = "day_trading_buying_power", Required = Required.Default)]
    public Decimal? DayTradingBuyingPower { get; set; }

    [JsonProperty(PropertyName = "max_dtbp_used", Required = Required.Default)]
    public Decimal? MaxDayTradingBuyingPowerUsed { get; set; }

    [JsonProperty(PropertyName = "max_dtbp_used_so_far", Required = Required.Default)]
    public Decimal? MaxDayTradingBuyingPowerUsedSoFar { get; set; }
}
