namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaDataClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static readonly String[] _symbols = { Stock, Other };

    private static readonly Interval<DateTime> _timeInterval;

    private const String PathPrefix = "/v2/stocks";

    private static readonly DateTime _yesterday;

    private static readonly DateTime _today;

    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    static AlpacaDataClientTest()
    {
        _today = DateTime.Today;
        _yesterday = _today.AddDays(-1);
        _timeInterval = new Interval<DateTime>(_yesterday, _today);
    }

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
                    new JProperty("source", Guid.NewGuid().ToString("D")),
                    new JProperty("author", Guid.NewGuid().ToString("D")),
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("updated_at", DateTime.UtcNow),
                    new JProperty("symbols", new JArray(Stock)),
                    new JProperty("id", 1234567890L)
                    )))));

        var articles = await mock.Client.ListNewsArticlesAsync(
            new NewsArticlesRequest
            {
                SortDirection = SortDirection.Ascending,
                ExcludeItemsWithoutContent = true,
                SendFullContentForItems = true
            });

        Assert.NotNull(articles.Symbol);
        var article = articles.Items.Single();
        Assert.Equal(1234567890L, article.Id);

        Assert.Equal(Stock, article.Symbols.Single());
        Assert.NotNull(article.LargeImageUrl);
        Assert.Null(article.SmallImageUrl);
        Assert.Null(article.ThumbImageUrl);
    }
}
