namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetLatestBarAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestBarExpectation(PathPrefix, Crypto);

        var bar = await mock.Client.GetLatestBarAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.True(bar.Validate(Crypto));
    }

    [Fact]
    public async Task ListLatestBarsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestBarsExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListLatestBarsAsync(
            new LatestDataListRequest(_symbols, CryptoExchange.Cbse));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars);

        Assert.True(bars[Crypto].Validate(Crypto));
        Assert.True(bars[Other].Validate(Other));
    }

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
    public async Task ListLatestQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestQuotesExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListLatestQuotesAsync(
            new LatestDataListRequest(_symbols, CryptoExchange.Cbse));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes);

        Assert.True(quotes[Crypto].Validate(Crypto));
        Assert.True(quotes[Other].Validate(Other));
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

    [Fact]
    public async Task ListLatestTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestTradesExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListLatestTradesAsync(
            new LatestDataListRequest(_symbols, CryptoExchange.Cbse));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades);

        Assert.True(trades[Crypto].Validate(Crypto));
        Assert.True(trades[Other].Validate(Other));
    }
}
