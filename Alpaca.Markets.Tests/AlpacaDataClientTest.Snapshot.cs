namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSnapshotExpectation(PathPrefix, Stock);

        var snapshot = await mock.Client.GetSnapshotAsync(new LatestMarketDataRequest(Stock)
        {
            Feed = MarketDataFeed.Otc,
            Currency = Currency
        });

        Assert.True(snapshot.Validate(Stock));
    }

    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client
            .ListSnapshotsAsync(new LatestMarketDataListRequest(_symbols)
            {
                Feed = MarketDataFeed.Otc,
                Currency = Currency
            });

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        Assert.True(snapshots[Stock].Validate(Stock));
        Assert.True(snapshots[Other].Validate(Other));
    }
}
