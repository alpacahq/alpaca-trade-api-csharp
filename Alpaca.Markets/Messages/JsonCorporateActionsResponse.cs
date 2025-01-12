namespace Alpaca.Markets;

[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(ICorporateActionsResponse))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonCorporateActionsResponse : ICorporateActionsResponse
{
    public sealed class JsonCorporateActionsDictionary
    {
        [JsonProperty(PropertyName = "stock_and_cash_mergers", Required = Required.Default)]
        public List<JsonStockAndCashMerger> StockAndCashMergersList { get; set; } = [];

        [JsonProperty(PropertyName = "rights_distributions", Required = Required.Default)]
        public List<JsonRightsDistribution> RightsDistributionsList { get; set; } = [];

        [JsonProperty(PropertyName = "worthless_removals", Required = Required.Default)]
        public List<JsonWorthlessRemoval> WorthlessRemovalsList { get; set; } = [];

        [JsonProperty(PropertyName = "stock_dividends", Required = Required.Default)]
        public List<JsonStockDividend> StockDividendsList { get; set; } = [];

        [JsonProperty(PropertyName = "cash_dividends", Required = Required.Default)]
        public List<JsonCashDividend> CashDividendsList { get; set; } = [];

        [JsonProperty(PropertyName = "reverse_splits", Required = Required.Default)]
        public List<JsonReverseSplit> ReverseSplitsList { get; set; } = [];

        [JsonProperty(PropertyName = "forward_splits", Required = Required.Default)]
        public List<JsonForwardSplit> ForwardSplitsList { get; set; } = [];

        [JsonProperty(PropertyName = "stock_mergers", Required = Required.Default)]
        public List<JsonStockMerger> StockMergersList { get; set; } = [];

        [JsonProperty(PropertyName = "cash_mergers", Required = Required.Default)]
        public List<JsonCashMerger> CashMergersList { get; set; } = [];

        [JsonProperty(PropertyName = "name_changes", Required = Required.Default)]
        public List<JsonNameChange> NameChangesList { get; set; } = [];

        [JsonProperty(PropertyName = "redemptions", Required = Required.Default)]
        public List<JsonRedemption> RedemptionsList { get; set; } = [];

        [JsonProperty(PropertyName = "unit_splits", Required = Required.Default)]
        public List<JsonUnitSplit> UnitSplitsList { get; set; } = [];

        [JsonProperty(PropertyName = "spin_offs", Required = Required.Default)]
        public List<JsonSpinOff> SpinOffsList { get; set; } = [];
    }

    [JsonProperty(PropertyName = "corporate_actions", Required = Required.Default)]
    public JsonCorporateActionsDictionary Nested { get; [ExcludeFromCodeCoverage] set; } = new();

    [JsonIgnore]
    public IReadOnlyList<IStockAndCashMerger> StockAndCashMergers => Nested.StockAndCashMergersList;

    [JsonIgnore]
    public IReadOnlyList<IRightsDistribution> RightsDistributions => Nested.RightsDistributionsList;

    [JsonIgnore]
    public IReadOnlyList<IWorthlessRemoval> WorthlessRemovals => Nested.WorthlessRemovalsList;

    [JsonIgnore]
    public IReadOnlyList<IStockDividend> StockDividends => Nested.StockDividendsList;

    [JsonIgnore]
    public IReadOnlyList<ICashDividend> CashDividends => Nested.CashDividendsList;

    [JsonIgnore]
    public IReadOnlyList<IReverseSplit> ReverseSplits => Nested.ReverseSplitsList;

    [JsonIgnore]
    public IReadOnlyList<IForwardSplit> ForwardSplits => Nested.ForwardSplitsList;

    [JsonIgnore]
    public IReadOnlyList<IStockMerger> StockMergers => Nested.StockMergersList;

    [JsonIgnore]
    public IReadOnlyList<INameChange> NameChanges => Nested.NameChangesList;

    [JsonIgnore]
    public IReadOnlyList<ICashMerger> CashMergers => Nested.CashMergersList;

    [JsonIgnore]
    public IReadOnlyList<IRedemption> Redemptions => Nested.RedemptionsList;

    [JsonIgnore]
    public IReadOnlyList<IUnitSplit> UnitSplits => Nested.UnitSplitsList;

    [JsonIgnore]
    public IReadOnlyList<ISpinOff> SpinOffs => Nested.SpinOffsList;

    [JsonProperty(PropertyName = "next_page_token", Required = Required.Default)]
    public String? NextPageToken { get; [ExcludeFromCodeCoverage] set;  }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(ICorporateActionsResponse)} {{ NextPageToken = {NextPageToken} }}";
}
