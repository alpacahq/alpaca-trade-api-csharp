namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleTradesPageExpectation);

        var counter = await validateList(
            mock.Client.GetHistoricalTradesAsAsyncEnumerable(
                new HistoricalOptionTradesRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalTradesDictionaryOfAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addMultiTradesPageExpectation);

        var counter = await validateDictionaryOfLists(
            mock.Client.GetHistoricalTradesDictionaryOfAsyncEnumerable(
                new HistoricalOptionTradesRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalTradesPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleTradesPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetHistoricalTradesPagesAsAsyncEnumerable(
                new HistoricalOptionTradesRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalTradesMultiPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addMultiTradesPageExpectation);

        var counter = await validateListOfDictionariesOfLists(
            mock.Client.GetHistoricalTradesMultiPagesAsAsyncEnumerable(
                new HistoricalOptionTradesRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    private static void addMultiTradesPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/options/trades", new JObject(
            new JProperty("trades", new JObject(
                new JProperty(Stock, createTradesList()),
                new JProperty(Other, createTradesList()))),
            new JProperty("next_page_token", token)));

    private static void addSingleTradesPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/options/trades", new JObject(
            new JProperty("trades", new JObject(
                new JProperty(Stock, createTradesList()))),
            new JProperty("next_page_token", token)));

    private static JArray createTradesList() =>
        new(createTrade(), createTrade(), createTrade());

    private static JObject createTrade() => new(
        new JProperty("t", DateTime.UtcNow),
        new JProperty("p", Price),
        new JProperty("s", Size));
}
