namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetOptionChainAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleContractPageExpectation);

        var counter = await validateList(
            mock.Client.GetOptionChainAsyncAsAsyncEnumerable(
                new OptionChainRequest(Stock)));

        Assert.NotEqual(0, counter);
    }

    [Fact]
    public async Task ListSnapshotsAsAsyncEnumerableWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        addPaginatedResponses(mock, addSingleSnapshotPageExpectation);

        var counter = await validateList(
            mock.Client.ListSnapshotsAsAsyncEnumerable(
                new OptionSnapshotRequest(_symbols)));

        Assert.NotEqual(0, counter);
    }

    private static void addSingleContractPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        addSingleContractOrSnapshotPageExpectation(mock, "/*", token);

    private static void addSingleSnapshotPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String? token = null) =>
        addSingleContractOrSnapshotPageExpectation(mock, String.Empty, token);

    private static void addSingleContractOrSnapshotPageExpectation(
        MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> mock,
        String urlPathLastSegment,
        String? token = null) =>
        mock.AddGet($"/v1beta1/options/snapshots{urlPathLastSegment}", new JObject(
            new JProperty("snapshots", new JObject(
                _symbols.Select(symbol => new JProperty(symbol, createSnapshot())).OfType<Object>().ToArray())),
            new JProperty("next_page_token", token)));

    private static JObject createSnapshot() => new(
        new JProperty("impliedVolatility", Price));
}