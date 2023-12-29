namespace Alpaca.Markets;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
[DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPortfolioHistory))]
[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonPortfolioHistory : IPortfolioHistory
{
    [DebuggerDisplay("{DebuggerDisplay,nq}", Type = nameof(IPortfolioHistoryItem))]
    private sealed class Item : IPortfolioHistoryItem
    {
        public Decimal? Equity { get; init; }

        public Decimal? ProfitLoss { get; init; }

        public Decimal? ProfitLossPercentage { get; init; }

        public DateTime TimestampUtc { get; init; }

        [ExcludeFromCodeCoverage]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private String DebuggerDisplay =>
            $"{nameof(IPortfolioHistoryItem)} {{ Time = {TimestampUtc:O}, Equity = {Equity}, ProfitLoss = {ProfitLoss} }}";
    }

    private readonly List<IPortfolioHistoryItem> _items = [];

    [JsonProperty(PropertyName = "equity", Required = Required.Always)]
    public List<Decimal?>? EquityList { get; set; }

    [JsonProperty(PropertyName = "profit_loss", Required = Required.Always)]
    public List<Decimal?>? ProfitLossList { get; set; }

    [JsonProperty(PropertyName = "profit_loss_pct", Required = Required.Always)]
    public List<Decimal?>? ProfitLossPercentageList { get; set; }

    [JsonProperty(PropertyName = "timestamp", Required = Required.Always,
        ItemConverterType = typeof(UnixSecondsDateTimeConverter))]
    public List<DateTime>? TimestampsList { get; set; }

    [JsonIgnore]
    public IReadOnlyList<IPortfolioHistoryItem> Items => _items;

    [JsonProperty(PropertyName = "timeframe", Required = Required.Always)]
    public TimeFrame TimeFrame { get; set; }

    [JsonProperty(PropertyName = "base_value", Required = Required.Always)]
    public Decimal BaseValue { get; set; }

    [OnDeserialized]
    [UsedImplicitly]
    internal void OnDeserializedMethod(
        StreamingContext _)
    {
        var equities = EquityList.EmptyIfNull();
        var timestamps = TimestampsList.EmptyIfNull();
        var profitLosses = ProfitLossList.EmptyIfNull();
        var profitLossesPercentage = ProfitLossPercentageList.EmptyIfNull();

        var count = Math.Min(
            Math.Min(equities.Count, timestamps.Count),
            Math.Min(profitLosses.Count, profitLossesPercentage.Count));

        for (var index = 0; index < count; ++index)
        {
            _items.Add(new Item
            {
                Equity = equities[index],
                ProfitLoss = profitLosses[index],
                TimestampUtc = timestamps[index],
                ProfitLossPercentage = profitLossesPercentage[index]
            });
        }
    }

    [ExcludeFromCodeCoverage]
    public override String ToString() =>
        JsonConvert.SerializeObject(this);

    [ExcludeFromCodeCoverage]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private String DebuggerDisplay =>
        $"{nameof(IPortfolioHistory)} {{ BaseValue = {BaseValue}, TimeFrame = {TimeFrame}, Count = {Items.Count} }}";
}
