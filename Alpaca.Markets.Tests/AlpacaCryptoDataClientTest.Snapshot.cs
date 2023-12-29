namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddCryptoSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.ListSnapshotsAsync(
            new SnapshotDataListRequest(_symbols));

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        Assert.True(snapshots[Crypto].Validate(Crypto));
        Assert.True(snapshots[Other].Validate(Other));
    }

    [Fact]
    public async Task ListLatestOrderBooksAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddOrderBooksExpectation(PathPrefix, _symbols);

#pragma warning disable CS0618
        var orderBooks = await mock.Client.ListLatestOrderBooksAsync(
            new LatestOrderBooksRequest(_symbols, _exchangesList));
#pragma warning restore CS0618

        Assert.NotNull(orderBooks);
        Assert.NotEmpty(orderBooks);

        Assert.True(HistoricalDataHelpers.Validate(orderBooks[Crypto], Crypto));
        Assert.True(HistoricalDataHelpers.Validate(orderBooks[Other], Other));
    }
}
