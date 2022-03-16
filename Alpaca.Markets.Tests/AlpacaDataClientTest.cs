namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaDataClientTest
{
    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static DateTime Yesterday => _timeInterval.From!.Value;

    private static readonly String[] _symbols = { Stock, Other };

    private static DateTime Today => _timeInterval.Into!.Value;

    private const String PathPrefix = "/v2/stocks";

    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    public AlpacaDataClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public void AlpacaDataClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaDataClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            _mockClientsFactory.GetAlpacaDataClientMock(nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            _mockClientsFactory.GetAlpacaDataClientMock(nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            _mockClientsFactory.GetAlpacaDataClientMock(nullThrottleParameters));
    }

    [Fact]
    public async Task ListNewsArticlesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v1beta1/news", new JObject(
            new JProperty("news", new JArray(
                new JObject(
                    new JProperty("images", new JArray(
                        new JObject(
                            new JProperty("url", new Uri("https://www.google.com")),
                            new JProperty("size", "large")))),
                    new JProperty("headline", Guid.NewGuid().ToString("D")),
                    new JProperty("content", Guid.NewGuid().ToString("D")),
                    new JProperty("summary", Guid.NewGuid().ToString("D")),
                    new JProperty("source", Guid.NewGuid().ToString("D")),
                    new JProperty("author", Guid.NewGuid().ToString("D")),
                    new JProperty("url", new Uri("https://www.google.com")),
                    new JProperty("id", Random.Shared.NextInt64()),
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("updated_at", DateTime.UtcNow),
                    new JProperty("symbols", new JArray(Stock))
                    )))));

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
        Assert.NotEqual(0L, article.Id);

        Assert.Equal(Stock, article.Symbols.Single());

        Assert.NotNull(article.ArticleUrl);
        Assert.NotNull(article.Headline);
        Assert.NotNull(article.Content);
        Assert.NotNull(article.Summary);
        Assert.NotNull(article.Author);
        Assert.NotNull(article.Source);

        Assert.NotNull(article.LargeImageUrl);
        Assert.Null(article.SmallImageUrl);
        Assert.Null(article.ThumbImageUrl);
    }

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
