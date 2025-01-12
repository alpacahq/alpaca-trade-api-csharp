namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaDataClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private static DateTime Yesterday => _timeInterval.From!.Value;

    private static readonly String[] _symbols = [ Stock, Other ];

    private static DateTime Today => _timeInterval.Into!.Value;

    private const String PathPrefix = "/v2/stocks";

    private const String Currency = "EUR";

    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    [Fact]
    public void AlpacaDataClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaDataClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaDataClientMock(Environments.Paper, nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaDataClientMock(Environments.Paper, nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaDataClientMock(Environments.Paper, nullThrottleParameters));
    }

    [Fact]
    public async Task ListNewsArticlesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock(Environments.Paper);

        mock.AddGet("/v1beta1/news", new JObject(
            new JProperty("news", new JArray(Stock.CreateNewsArticle()))));

        var articles = await mock.Client.ListNewsArticlesAsync(
            new NewsArticlesRequest
            {
                SortDirection = SortDirection.Ascending,
                ExcludeItemsWithoutContent = true,
                SendFullContentForItems = true
            });

        Assert.NotNull(articles.Symbol);
        Assert.Null(articles.NextPageToken);

        var article = articles.Items.Single();
        article.Validate(Stock);
    }

    [Fact]
    public async Task ListCorporateActionsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock(Environments.Paper);

        mock.AddGet("/v1/corporate-actions", new JObject(
            new JProperty("corporate_actions", new JObject(
                new JProperty("stock_and_cash_mergers", new JArray(Stock.CreateStockAndCashMerger())),
                new JProperty("rights_distributions", new JArray(Stock.CreateRightsDistribution())),
                new JProperty("worthless_removals", new JArray(Stock.CreateWorthlessRemoval())),
                new JProperty("stock_dividends", new JArray(Stock.CreateStockDividend())),
                new JProperty("cash_dividends", new JArray(Stock.CreateCashDividend())),
                new JProperty("reverse_splits", new JArray(Stock.CreateReverseSplit())),
                new JProperty("forward_splits", new JArray(Stock.CreateForwardSplit())),
                new JProperty("stock_mergers", new JArray(Stock.CreateStockMerger())),
                new JProperty("cash_mergers", new JArray(Stock.CreateCashMerger())),
                new JProperty("name_changes", new JArray(Stock.CreateNameChange())),
                new JProperty("redemptions", new JArray(Stock.CreateRedemption())),
                new JProperty("unit_splits", new JArray(Stock.CreateUnitSplit())),
                new JProperty("spin_offs", new JArray(Stock.CreateSpinOff()))))));

        var today = DateOnly.FromDateTime(DateTime.Today);
        var request = new CorporateActionsRequest
            {
                SortDirection = SortDirection.Ascending
            }
            .WithDateInterval(new Interval<DateOnly>(today, today))
            .WithTypes([CorporateActionFilterType.ForwardSplit])
            .WithType(CorporateActionFilterType.SpinOff)
            .WithSymbols([Stock]).WithSymbol(Stock);

        var actions= await mock.Client.ListCorporateActionsAsync(request);

        Assert.Null(actions.NextPageToken);

        getCorporateAction(actions.StockAndCashMergers).Validate(Stock);
        getCorporateAction(actions.RightsDistributions).Validate(Stock);
        getCorporateAction(actions.WorthlessRemovals).Validate(Stock);
        getCorporateAction(actions.StockDividends).Validate(Stock);
        getCorporateAction(actions.CashDividends).Validate(Stock);
        getCorporateAction(actions.ReverseSplits).Validate(Stock);
        getCorporateAction(actions.ForwardSplits).Validate(Stock);
        getCorporateAction(actions.StockMergers).Validate(Stock);
        getCorporateAction(actions.CashMergers).Validate(Stock);
        getCorporateAction(actions.NameChanges).Validate(Stock);
        getCorporateAction(actions.Redemptions).Validate(Stock);
        getCorporateAction(actions.UnitSplits).Validate(Stock);
        getCorporateAction(actions.SpinOffs).Validate(Stock);
    }

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
    private static TAction getCorporateAction<TAction>(
        IReadOnlyList<TAction> actions)
    {
        Assert.NotNull(actions);
        Assert.Single(actions);
        return actions.Single();
    }
}
