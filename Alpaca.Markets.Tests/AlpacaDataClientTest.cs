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

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
