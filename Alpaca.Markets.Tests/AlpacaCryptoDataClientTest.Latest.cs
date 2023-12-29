namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task ListLatestBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestCryptoBarsExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListLatestBarsAsync(
            new LatestDataListRequest(_symbols));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars);

        Assert.True(bars[Crypto].Validate(Crypto));
        Assert.True(bars[Other].Validate(Other));
    }

    [Fact]
    public async Task ListLatestQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestCryptoQuotesExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListLatestQuotesAsync(
            new LatestDataListRequest(_symbols));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes);

        Assert.True(quotes[Crypto].Validate(Crypto));
        Assert.True(quotes[Other].Validate(Other));
    }

    [Fact]
    public async Task ListLatestTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddLatestCryptoTradesExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListLatestTradesAsync(
            new LatestDataListRequest(_symbols));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades);

        Assert.True(trades[Crypto].Validate(Crypto));
        Assert.True(trades[Other].Validate(Other));
    }
}
