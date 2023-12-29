namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetLatestBarAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestBarExpectation(PathPrefix, Stock);

        var bar = await mock.Client.GetLatestBarAsync(new LatestMarketDataRequest(Stock));

        Assert.True(bar.Validate(Stock));
    }

    [Fact]
    public async Task ListLatestBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestBarsExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListLatestBarsAsync(
            new LatestMarketDataListRequest(_symbols));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars);

        Assert.True(bars[Stock].Validate(Stock));
        Assert.True(bars[Other].Validate(Other));
    }

    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestQuoteExpectation(PathPrefix, Stock);

        var quote = await mock.Client.GetLatestQuoteAsync(new LatestMarketDataRequest(Stock));

        Assert.True(quote.Validate(Stock));
    }

    [Fact]
    public async Task ListLatestQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestQuotesExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListLatestQuotesAsync(
            new LatestMarketDataListRequest(_symbols));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes);

        Assert.True(quotes[Stock].Validate(Stock));
        Assert.True(quotes[Other].Validate(Other));
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestTradeExpectation(PathPrefix, Stock);

        var trade = await mock.Client.GetLatestTradeAsync(new LatestMarketDataRequest(Stock));

        Assert.True(trade.Validate(Stock));
    }

    [Fact]
    public async Task ListLatestTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddLatestTradesExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListLatestTradesAsync(
            new LatestMarketDataListRequest(_symbols));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes);

        Assert.True(quotes[Stock].Validate(Stock));
        Assert.True(quotes[Other].Validate(Other));
    }
}
