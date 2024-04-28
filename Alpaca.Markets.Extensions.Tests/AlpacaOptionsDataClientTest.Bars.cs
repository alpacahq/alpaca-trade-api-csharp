namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetHistoricalBarsAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var counter = await validateList(
            mock.Client.GetHistoricalBarsAsAsyncEnumerable(
                new HistoricalOptionBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsDictionaryOfAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addMultiBarsPageExpectation);

        var counter = await validateDictionaryOfLists(
            mock.Client.GetHistoricalBarsDictionaryOfAsyncEnumerable(
                new HistoricalOptionBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetHistoricalBarsPagesAsAsyncEnumerable(
                new HistoricalOptionBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsMultiPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addMultiBarsPageExpectation);

        var counter = await validateListOfDictionariesOfLists(
            mock.Client.GetHistoricalBarsMultiPagesAsAsyncEnumerable(
                new HistoricalOptionBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)));

        Assert.NotEqual(0, counter);
    }

    private static void addMultiBarsPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/options/bars", new JObject(
            new JProperty("bars", new JObject(
                new JProperty(Stock, createBarsList()),
                new JProperty(Other, createBarsList()))),
            new JProperty("next_page_token", token)));

    private static void addSingleBarsPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/options/bars", new JObject(
            new JProperty("bars", new JObject(
                new JProperty(Stock, createBarsList()))),
            new JProperty("next_page_token", token)));

    private static JArray createBarsList() =>
        new(createBar(), createBar(), createBar());

    private static JObject createBar() => new(
        new JProperty("t", DateTime.UtcNow),
        new JProperty("o", Price),
        new JProperty("l", Price),
        new JProperty("h", Price),
        new JProperty("c", Price),
        new JProperty("v", Volume));
}
