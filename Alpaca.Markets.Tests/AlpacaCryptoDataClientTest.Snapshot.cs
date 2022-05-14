namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddSnapshotExpectation(PathPrefix, Crypto);

        var snapshot = await mock.Client.GetSnapshotAsync(
            new SnapshotDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.True(snapshot.Validate(Crypto));
    }

    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddCryptoSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.ListSnapshotsAsync(
            new SnapshotDataListRequest(_symbols, CryptoExchange.Cbse));

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        Assert.True(snapshots[Crypto].Validate(Crypto));
        Assert.True(snapshots[Other].Validate(Other));
    }

    [Fact]
    public async Task GetLatestBestBidOfferAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbboExpectation(PathPrefix, Crypto);

        var bbo = await mock.Client.GetLatestBestBidOfferAsync(
            new LatestBestBidOfferRequest(Crypto, _exchangesList));

        Assert.True(bbo.Validate(Crypto));
    }

    [Fact]
    public async Task ListLatestBestBidOffersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbbosExpectation(PathPrefix, _symbols);

        var xbbos = await mock.Client.ListLatestBestBidOffersAsync(
            new LatestBestBidOfferListRequest(_symbols, _exchangesList));

        Assert.NotNull(xbbos);
        Assert.NotEmpty(xbbos);

        Assert.True(xbbos[Crypto].Validate(Crypto));
        Assert.True(xbbos[Other].Validate(Other));
    }

    [Fact]
    public async Task ListLatestBestBidOffersAsyncForSingleExchangeWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbbosExpectation(PathPrefix, _symbols);

        var xbbos = await mock.Client.ListLatestBestBidOffersAsync(
            new LatestBestBidOfferListRequest(_symbols, CryptoExchange.Ersx));

        Assert.NotNull(xbbos);
        Assert.NotEmpty(xbbos);

        Assert.True(xbbos[Crypto].Validate(Crypto));
        Assert.True(xbbos[Other].Validate(Other));
    }
}
