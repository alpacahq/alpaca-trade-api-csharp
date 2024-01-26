namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IOptionContract))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonOptionContract : IOptionContract
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid ContractId { get; set; }

    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Symbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "name", Required = Required.Always)]
    public String Name { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "status", Required = Required.Always)]
    public AssetStatus Status { get; set; }

    [JsonProperty(PropertyName = "tradable", Required = Required.Always)]
    public Boolean IsTradable { get; set; }

    [JsonProperty(PropertyName = "size", Required = Required.Always)]
    public Decimal Size { get; set; }

    [JsonProperty(PropertyName = "type", Required = Required.Always)]
    public OptionType OptionType { get; }

    [JsonProperty(PropertyName = "strike_price", Required = Required.Always)]
    public Decimal StrikePrice { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "expiration_date", Required = Required.Always)]
    public DateOnly ExpirationDate { get; set; }

    [JsonProperty(PropertyName = "style", Required = Required.Always)]
    public OptionStyle OptionStyle { get; }

    [JsonProperty(PropertyName = "root_symbol", Required = Required.Always)]
    public String RootSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "underlying_symbol", Required = Required.Always)]
    public String UnderlyingSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "underlying_asset_id", Required = Required.Always)]
    public Guid UnderlyingAssetId { get; set; }

    [JsonProperty(PropertyName = "open_interest", Required = Required.Default)]
    public Decimal? OpenInterest { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "open_interest_date", Required = Required.Default)]
    public DateOnly? OpenInterestDate { get; set; }

    [JsonProperty(PropertyName = "close_price", Required = Required.Default)]
    public Decimal? ClosePrice { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "close_price_date", Required = Required.Default)]
    public DateOnly? ClosePriceDate { get; set; }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IOptionContract)} {{ ID = {ContractId:B}, Symbol = \"{Symbol}\" }}";
}
