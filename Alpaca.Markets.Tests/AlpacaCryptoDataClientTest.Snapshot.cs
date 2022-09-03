namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddSnapshotExpectation(OldPathPrefix, Crypto);

#pragma warning disable CS0618
        var snapshot = await mock.Client.GetSnapshotAsync(
            new SnapshotDataRequest(Crypto, CryptoExchange.Cbse));
#pragma warning restore CS0618

        Assert.True(snapshot.Validate(Crypto));
    }

    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

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
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

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

    [Fact]
    public async Task GetLatestBestBidOfferAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbboExpectation(OldPathPrefix, Crypto);

#pragma warning disable CS0618
        var bbo = await mock.Client.GetLatestBestBidOfferAsync(
            new LatestBestBidOfferRequest(Crypto, _exchangesList));
#pragma warning restore CS0618

        Assert.True(bbo.Validate(Crypto));
    }

    [Fact]
    public async Task ListLatestBestBidOffersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbbosExpectation(OldPathPrefix, _symbols);

#pragma warning disable CS0618
        var xbbos = await mock.Client.ListLatestBestBidOffersAsync(
            new LatestBestBidOfferListRequest(_symbols, _exchangesList));
#pragma warning restore CS0618

        Assert.NotNull(xbbos);
        Assert.NotEmpty(xbbos);

        Assert.True(xbbos[Crypto].Validate(Crypto));
        Assert.True(xbbos[Other].Validate(Other));
    }

    [Fact]
    public async Task ListLatestBestBidOffersAsyncForSingleExchangeWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbbosExpectation(OldPathPrefix, _symbols);

#pragma warning disable CS0618
        var xbbos = await mock.Client.ListLatestBestBidOffersAsync(
            new LatestBestBidOfferListRequest(_symbols, CryptoExchange.Ersx));
#pragma warning restore CS0618

        Assert.NotNull(xbbos);
        Assert.NotEmpty(xbbos);

        Assert.True(xbbos[Crypto].Validate(Crypto));
        Assert.True(xbbos[Other].Validate(Other));
    }
}
