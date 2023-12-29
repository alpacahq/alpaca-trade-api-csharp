namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleQuotesPageExpectation);

        var counter = await validateList(
            mock.Client.GetHistoricalQuotesAsAsyncEnumerable(
                new HistoricalQuotesRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalQuotesDictionaryOfAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiQuotesPageExpectation);

        var counter = await validateDictionaryOfLists(
            mock.Client.GetHistoricalQuotesDictionaryOfAsyncEnumerable(
                new HistoricalQuotesRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalQuotesPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleQuotesPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetHistoricalQuotesPagesAsAsyncEnumerable(
                new HistoricalQuotesRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalQuotesMultiPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiQuotesPageExpectation);

        var counter = await validateListOfDictionariesOfLists(
            mock.Client.GetHistoricalQuotesMultiPagesAsAsyncEnumerable(
                new HistoricalQuotesRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    private static void addMultiQuotesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/quotes", new JObject(
            new JProperty("quotes", new JObject(
                new JProperty(Stock, createQuotesList()),
                new JProperty(Other, createQuotesList()))),
            new JProperty("next_page_token", token)));

    private static void addSingleQuotesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/**/quotes", new JObject(
            new JProperty("quotes", createQuotesList()),
            new JProperty("next_page_token", token),
            new JProperty("symbol", Stock)));

    private static JArray createQuotesList() =>
        new(createQuote(), createQuote(), createQuote());

    private static JObject createQuote() => new(
        new JProperty("t", DateTime.UtcNow),
        new JProperty("ax", "A"),
        new JProperty("bx", "B"));
}
