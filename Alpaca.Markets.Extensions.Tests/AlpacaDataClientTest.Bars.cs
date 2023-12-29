namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetAverageDailyTradeVolumeAsyncWithDatesWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var (from, into) = _timeInterval.AsDateInterval();
        var (adtv, count) = await mock.Client.GetAverageDailyTradeVolumeAsync(
            Stock, from!.Value, into!.Value);

        Assert.Equal(Volume, adtv);
        Assert.True(count != 0);
    }

    [Fact]
    public async Task GetAverageDailyTradeVolumeAsyncWithIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var (adtv, count) = await mock.Client.GetAverageDailyTradeVolumeAsync(
            Stock, _timeInterval.AsDateInterval());

        Assert.Equal(Volume, adtv);
        Assert.True(count != 0);
    }

    [Fact]
    public async Task GetSimpleMovingAverageAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var counter = await validateList(
            mock.Client.GetSimpleMovingAverageAsync(
                new HistoricalBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval), 3));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetSimpleMovingAverageAsyncZeroWindowWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        var counter = await validateList(
            mock.Client.GetSimpleMovingAverageAsync(
                new HistoricalBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval), 0));

        Assert.Equal(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var counter = await validateList(
            mock.Client.GetHistoricalBarsAsAsyncEnumerable(
                new HistoricalBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsDictionaryOfAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiBarsPageExpectation);

        var counter = await validateDictionaryOfLists(
            mock.Client.GetHistoricalBarsDictionaryOfAsyncEnumerable(
                new HistoricalBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetHistoricalBarsPagesAsAsyncEnumerable(
                new HistoricalBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalBarsMultiPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiBarsPageExpectation);

        var counter = await validateListOfDictionariesOfLists(
            mock.Client.GetHistoricalBarsMultiPagesAsAsyncEnumerable(
                new HistoricalBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)));

        Assert.NotEqual(0, counter);
    }

    private static void addMultiBarsPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/bars", new JObject(
            new JProperty("bars", new JObject(
                new JProperty(Stock, createBarsList()),
                new JProperty(Other, createBarsList()))),
            new JProperty("next_page_token", token)));

    private static void addSingleBarsPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/**/bars", new JObject(
            new JProperty("bars", createBarsList()),
            new JProperty("next_page_token", token),
            new JProperty("symbol", Stock)));

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
