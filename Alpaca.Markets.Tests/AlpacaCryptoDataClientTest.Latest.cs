namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestQuoteExpectation(PathPrefix, Crypto);

        var quote = await mock.Client.GetLatestQuoteAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.True(quote.Validate(Crypto));
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestTradeExpectation(PathPrefix, Crypto);

        var trade = await mock.Client.GetLatestTradeAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Ersx));

        Assert.True(trade.Validate(Crypto));
    }
}
