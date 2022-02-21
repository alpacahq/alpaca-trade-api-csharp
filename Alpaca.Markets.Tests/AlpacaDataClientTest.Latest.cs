namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestQuoteExpectation(PathPrefix, Stock);

        var quote = await mock.Client.GetLatestQuoteAsync(Stock);

        Assert.True(quote.Validate(Stock));
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestTradeExpectation(PathPrefix, Stock);

        var trade = await mock.Client.GetLatestTradeAsync(Stock);

        Assert.True(trade.Validate(Stock));
    }
}
