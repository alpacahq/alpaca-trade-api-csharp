namespace Alpaca.Markets;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAccount))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAccount : IAccount
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid AccountId { get; set; }

    [JsonProperty(PropertyName = "account_number", Required = Required.Default)]
    public String? AccountNumber { get; set; }

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public AccountStatus Status { get; set; }

    [JsonProperty(PropertyName = "crypto_status", Required = Required.Default)]
    public AccountStatus CryptoStatus { get; set; }

    [JsonProperty(PropertyName = "currency", Required = Required.Default)]
    public String? Currency { get; set; }

    [JsonProperty(PropertyName = "cash", Required = Required.Always)]
    public Decimal TradableCash { get; set; }

    [JsonProperty(PropertyName = "pattern_day_trader", Required = Required.Always)]
    public Boolean IsDayPatternTrader { get; set; }

    [JsonProperty(PropertyName = "trading_blocked", Required = Required.Always)]
    public Boolean IsTradingBlocked { get; set; }

    [JsonProperty(PropertyName = "transfers_blocked", Required = Required.Always)]
    public Boolean IsTransfersBlocked { get; set; }

    [JsonProperty(PropertyName = "account_blocked", Required = Required.Always)]
    public Boolean IsAccountBlocked { get; set; }

    [JsonProperty(PropertyName = "trade_suspended_by_user", Required = Required.Default)]
    public Boolean TradeSuspendedByUser { get; set; }

    [JsonProperty(PropertyName = "shorting_enabled", Required = Required.Default)]
    public Boolean ShortingEnabled { get; set; }

    [JsonProperty(PropertyName = "multiplier", Required = Required.Default)]
    public Multiplier Multiplier { get; set; }

    [JsonProperty(PropertyName = "buying_power", Required = Required.Default)]
    public Decimal? BuyingPower { get; set; }

    [JsonProperty(PropertyName = "daytrading_buying_power", Required = Required.Default)]
    public Decimal? DayTradingBuyingPower { get; set; }

    [JsonProperty(PropertyName = "non_maginable_buying_power", Required = Required.Default)]
    public Decimal? NonMarginableBuyingPower { get; set; }

    [JsonProperty(PropertyName = "regt_buying_power", Required = Required.Default)]
    public Decimal? RegulationBuyingPower { get; set; }

    [JsonProperty(PropertyName = "long_market_value", Required = Required.Default)]
    public Decimal? LongMarketValue { get; set; }

    [JsonProperty(PropertyName = "short_market_value", Required = Required.Default)]
    public Decimal? ShortMarketValue { get; set; }

    [JsonProperty(PropertyName = "equity", Required = Required.Default)]
    public Decimal? Equity { get; set; }

    [JsonProperty(PropertyName = "last_equity", Required = Required.Default)]
    public Decimal LastEquity { get; set; }

    [JsonProperty(PropertyName = "initial_margin", Required = Required.Default)]
    public Decimal? InitialMargin { get; set; }

    [JsonProperty(PropertyName = "maintenance_margin", Required = Required.Default)]
    public Decimal MaintenanceMargin { get; set; }

    [JsonProperty(PropertyName = "last_maintenance_margin", Required = Required.Default)]
    public Decimal LastMaintenanceMargin { get; set; }

    [JsonProperty(PropertyName = "daytrade_count", Required = Required.Default)]
    public UInt64 DayTradeCount { get; set; }

    [JsonProperty(PropertyName = "sma", Required = Required.Default)]
    public Decimal Sma { get; set; }

    [JsonConverter(typeof(AssumeUtcIsoDateTimeConverter))]
    [JsonProperty(PropertyName = "created_at", Required = Required.Always)]
    public DateTime CreatedAtUtc { get; set; }

    [JsonProperty(PropertyName = "accrued_fees", Required = Required.Default)]
    public Decimal? AccruedFees { get; set; }

    [JsonProperty(PropertyName = "pending_transfer_in", Required = Required.Default)]
    public Decimal? PendingTransferIn { get; set; }

    [JsonProperty(PropertyName = "pending_transfer_out", Required = Required.Default)]
    public Decimal? PendingTransferOut { get; set; }

    [JsonProperty(PropertyName = "options_trading_level", Required = Required.Default)]
    public OptionsTradingLevel? OptionsTradingLevel { get; set; }

    [JsonProperty(PropertyName = "options_approved_level", Required = Required.Default)]
    public OptionsTradingLevel? OptionsApprovedLevel { get; set; }

    [JsonProperty(PropertyName = "options_buying_power", Required = Required.Default)]
    public Decimal? OptionsBuyingPower { get; set; }

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        if (String.IsNullOrEmpty(Currency))
        {
            Currency = "USD";
        }
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IAccount)} {{ ID = {AccountId:B}, Number = \"{AccountNumber}\", Status = {Status}, Currency = \"{Currency}\" }}";
}
