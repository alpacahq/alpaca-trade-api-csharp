namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/**/quotes/latest", 
            new JsonLatestQuote<JsonHistoricalQuote> { Symbol = Stock });

        var quote = await mock.Client.GetLatestQuoteAsync(Stock);

        Assert.Equal(Stock, quote.Symbol);
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/**/trades/latest",
            new JsonLatestTrade { Symbol = Stock });

        var trade = await mock.Client.GetLatestTradeAsync(Stock);

        Assert.Equal(Stock, trade.Symbol);
    }
}
