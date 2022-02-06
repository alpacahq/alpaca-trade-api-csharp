using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddGet("/v1beta1/crypto/**/quotes/latest", 
            new JsonLatestQuote<JsonHistoricalCryptoQuote> { Symbol = Crypto });

        var quote = await mock.Client.GetLatestQuoteAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.Equal(Crypto, quote.Symbol);
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddGet("/v1beta1/crypto/**/trades/latest",
            new JsonLatestTrade { Symbol = Crypto });

        var trade = await mock.Client.GetLatestTradeAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Ersx));

        Assert.Equal(Crypto, trade.Symbol);
    }
}
