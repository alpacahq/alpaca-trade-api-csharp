using Xunit;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaDataClientTest
{
    private static readonly String _condition = Guid.NewGuid().ToString("D");

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static readonly String[] _symbols = { Stock, Other };

    private static readonly Interval<DateTime> _timeInterval;

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

        mock.AddGet("/v1beta1/news", new JsonNewsPage
        {
            ItemsList = new List<JsonNewsArticle>
            {
                new ()
                {
                    Images = new List<JsonNewsArticle.Image>
                    {
                        new ()
                        {
                            Url = new Uri("https://www.google.com"),
                            Size = "large"
                        }
                    },
                    SymbolsList = new List<String> { Stock },
                    Id = 1234567890L
                }
            }
        });

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
