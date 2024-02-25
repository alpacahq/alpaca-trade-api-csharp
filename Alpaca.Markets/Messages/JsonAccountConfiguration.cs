namespace Alpaca.Markets;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IAccountConfiguration))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAccountConfiguration : IAccountConfiguration
{
    [JsonProperty(PropertyName = "dtbp_check", Required = Required.Always)]
    public DayTradeMarginCallProtection DayTradeMarginCallProtection { get; set; }

    [JsonProperty(PropertyName = "trade_confirm_email", Required = Required.Always)]
    public TradeConfirmEmail TradeConfirmEmail { get; set; }

    [JsonProperty(PropertyName = "suspend_trade", Required = Required.Always)]
    public Boolean IsSuspendTrade { get; set; }

    [JsonProperty(PropertyName = "no_shorting", Required = Required.Always)]
    public Boolean IsNoShorting { get; set; }

    [JsonProperty(PropertyName = "ptp_no_exception_entry", Required = Required.Default)]
    public Boolean IsPtpNoExceptionEntry { get; set; }

    [JsonProperty(PropertyName = "max_options_trading_level", Required = Required.Default)]
    public OptionsTradingLevel? MaxOptionsTradingLevel { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IAccountConfiguration)} {{ DTMCP = {DayTradeMarginCallProtection}, IsSuspendTrade = {IsSuspendTrade}, IsNoShorting = {IsNoShorting} }}";
}
