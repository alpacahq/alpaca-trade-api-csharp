namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetNewsArticlesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleNewsPageExpectation);

        var counter = await validateList(
            mock.Client.GetNewsArticlesAsAsyncEnumerable(
                new NewsArticlesRequest(_symbols)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetNewsArticlesPagesAsAsyncEnumerable()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleNewsPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetNewsArticlesPagesAsAsyncEnumerable(
                new NewsArticlesRequest(_symbols)));

        Assert.NotEqual(0, counter);
    }

    private static void addSingleNewsPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/news", new JObject(
            new JProperty("news", createNewsList()),
            new JProperty("next_page_token", token),
            new JProperty("symbol", Stock)));

    private static JArray createNewsList() =>
        new(createNewsArticle(), createNewsArticle(), createNewsArticle());

    private static JObject createNewsArticle() => new(
        new JProperty("headline", Guid.NewGuid().ToString("D")),
        new JProperty("author", Guid.NewGuid().ToString("D")),
        new JProperty("source", Guid.NewGuid().ToString("D")),
        new JProperty("symbols", new JArray(Stock, Other)),
        new JProperty("id", Random.Shared.NextInt64()),
        new JProperty("created_at", DateTime.UtcNow),
        new JProperty("updated_at", DateTime.UtcNow));
}
