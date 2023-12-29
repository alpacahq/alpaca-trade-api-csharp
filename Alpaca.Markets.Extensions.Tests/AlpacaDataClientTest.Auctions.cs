namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalAuctionsAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleAuctionsPageExpectation);

        var counter = await validateList(
            mock.Client.GetHistoricalAuctionsAsAsyncEnumerable(
                new HistoricalAuctionsRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalAuctionsDictionaryOfAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiAuctionsPageExpectation);

        var counter = await validateDictionaryOfLists(
            mock.Client.GetHistoricalAuctionsDictionaryOfAsyncEnumerable(
                new HistoricalAuctionsRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalAuctionsPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addSingleAuctionsPageExpectation);

        var counter = await validateListOfLists(
            mock.Client.GetHistoricalAuctionsPagesAsAsyncEnumerable(
                new HistoricalAuctionsRequest(Stock, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task GetHistoricalAuctionsMultiPagesAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        addPaginatedResponses(mock, addMultiAuctionsPageExpectation);

        var counter = await validateListOfDictionariesOfLists(
            mock.Client.GetHistoricalAuctionsMultiPagesAsAsyncEnumerable(
                new HistoricalAuctionsRequest(_symbols, _timeInterval)));

        Assert.NotEqual(0, counter);
    }

    private static void addMultiAuctionsPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/auctions", new JObject(
            new JProperty("auctions", new JObject(
                new JProperty(Stock, createAuctionsList()),
                new JProperty(Other, createAuctionsList()))),
            new JProperty("next_page_token", token)));

    private static void addSingleAuctionsPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v2/stocks/**/auctions", new JObject(
            new JProperty("auctions", createAuctionsList()),
            new JProperty("next_page_token", token),
            new JProperty("symbol", Stock)));

    private static JArray createAuctionsList() =>
        new(createAuction(), createAuction(), createAuction());

    private static JObject createAuction() => new(
        new JProperty("d", DateTime.UtcNow),
        new JProperty("o", new JArray(createAuctionEntry(), createAuctionEntry())),
        new JProperty("c", new JArray(createAuctionEntry(), createAuctionEntry())));

    private static JObject createAuctionEntry() => new(
        new JProperty("t", DateTime.UtcNow),
        new JProperty("c", String.Empty),
        new JProperty("p", Price),
        new JProperty("s", Size));
}
