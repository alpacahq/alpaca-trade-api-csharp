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
    public async Task GetLatestBestBidOfferAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddXbboExpectation(PathPrefix, Crypto);

        var bbo = await mock.Client.GetLatestBestBidOfferAsync(
            new LatestBestBidOfferRequest(Crypto, new [] { CryptoExchange.Ersx, CryptoExchange.Gnss }));

        Assert.True(bbo.Validate(Crypto));
    }
}
