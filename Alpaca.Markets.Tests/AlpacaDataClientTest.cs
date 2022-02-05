using Xunit;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaDataClientTest
{
    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    public AlpacaDataClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ListIntervalCalendarAsyncWorks()
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
