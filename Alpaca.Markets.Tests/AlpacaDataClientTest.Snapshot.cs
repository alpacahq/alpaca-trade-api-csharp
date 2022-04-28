namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSnapshotExpectation(PathPrefix, Stock);

        var snapshot = await mock.Client.GetSnapshotAsync(new LatestMarketDataRequest(Stock)
        {
            Feed = MarkedDataFeed.Otc
        });

        Assert.True(snapshot.Validate(Stock));
    }

    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client
            .ListSnapshotsAsync(new LatestMarketDataListRequest(_symbols)
            {
                Feed = MarkedDataFeed.Otc
            });

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        Assert.True(snapshots[Stock].Validate(Stock));
        Assert.True(snapshots[Other].Validate(Other));
    }
}
