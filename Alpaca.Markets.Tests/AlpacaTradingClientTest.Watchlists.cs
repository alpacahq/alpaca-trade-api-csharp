namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const String WatchListsWildcardUrl = $"{WatchlistsUrl}/**";

    private const String WatchlistsByNameUrl = $"{WatchlistsUrl}:by_name";

    private const String WatchlistsUrl = "/v2/watchlists";

    [Fact]
    public async Task ListWatchListsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet(WatchlistsUrl, new JArray(createWatchList()));

        var watchLists = await mock.Client.ListWatchListsAsync();

        Assert.NotNull(watchLists);
        Assert.NotEmpty(watchLists);

        validateWatchList(watchLists.Single());
    }

    [Fact]
    public async Task GetWatchListByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet(WatchListsWildcardUrl, createWatchList());

        var watchList = await mock.Client.GetWatchListByIdAsync(Guid.NewGuid());

        validateWatchList(watchList);
    }

    [Fact]
    public async Task GetWatchListByNameAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet(WatchlistsByNameUrl, createWatchList());

        var watchList = await mock.Client.GetWatchListByNameAsync(Guid.NewGuid().ToString("D"));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task CreateWatchListAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(WatchlistsUrl, createWatchList());

        var watchList = await mock.Client.CreateWatchListAsync(new NewWatchListRequest(
            Guid.NewGuid().ToString("D"), new [] { Stock, Crypto }));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task UpdateWatchListByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPut(WatchListsWildcardUrl, createWatchList());

        var watchList = await mock.Client.UpdateWatchListByIdAsync(new UpdateWatchListRequest(
            Guid.NewGuid(), Guid.NewGuid().ToString("D"), new [] { Stock, Crypto }));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task AddAssetIntoWatchListByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(WatchListsWildcardUrl, createWatchList());

        var watchList = await mock.Client.AddAssetIntoWatchListByIdAsync(
            new ChangeWatchListRequest<Guid>(Guid.NewGuid(), Stock));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task AddAssetIntoWatchListByNameAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(WatchlistsByNameUrl, createWatchList());

        var watchList = await mock.Client.AddAssetIntoWatchListByNameAsync(
            new ChangeWatchListRequest<String>(Guid.NewGuid().ToString("D"), Stock));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task DeleteAssetFromWatchListByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete(WatchListsWildcardUrl, createWatchList());

        var watchList = await mock.Client.DeleteAssetFromWatchListByIdAsync(
            new ChangeWatchListRequest<Guid>(Guid.NewGuid(), Stock));

        validateWatchList(watchList);
    }

    [Fact]
    public async Task DeleteAssetFromWatchListByNameAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete($"{WatchlistsByNameUrl}/**", createWatchList());

        var watchList = await mock.Client.DeleteAssetFromWatchListByNameAsync(
            new ChangeWatchListRequest<String>(Guid.NewGuid().ToString("D"), Stock));

        validateWatchList(watchList);
    }
    [Fact]
    public async Task DeleteWatchListByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete(WatchListsWildcardUrl, createWatchList());

        Assert.True(await mock.Client.DeleteWatchListByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteWatchListByNameAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete(WatchlistsByNameUrl, createWatchList());

        Assert.True(await mock.Client.DeleteWatchListByNameAsync(Guid.NewGuid().ToString("D")));
    }

    private static JToken createWatchList() =>
        new JObject(
            new JProperty("name", Guid.NewGuid().ToString("D")),
            new JProperty("created_at", DateTime.UtcNow),
            new JProperty("updated_at", DateTime.UtcNow),
            new JProperty("account_id", Guid.NewGuid()),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("assets", new JArray(
                new JObject(
                    new JProperty("name", Guid.NewGuid().ToString()),
                    new JProperty("status", AssetStatus.Active),
                    new JProperty("exchange", Exchange.Nyse),
                    new JProperty("id", Guid.NewGuid()),
                    new JProperty("tradable", true),
                    new JProperty("symbol", Stock)))));

    private static void validateWatchList(IWatchList watchList)
    {
        Assert.NotNull(watchList);

        Assert.NotEqual(Guid.Empty, watchList.WatchListId);
        Assert.NotEqual(Guid.Empty, watchList.AccountId);
        Assert.NotNull(watchList.Name);

        Assert.NotNull(watchList.Assets);
        Assert.NotEmpty(watchList.Assets);

        var asset = watchList.Assets.Single();

        Assert.NotNull(asset);
        Assert.Equal(Stock, asset.Symbol);
    }
}
