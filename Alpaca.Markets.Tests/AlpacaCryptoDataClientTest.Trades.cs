namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Crypto].Validate(Crypto);
        trades.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbol);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Crypto].Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbol);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Crypto, trades.Symbol);

        trades.Items.Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        trades.Items.Where(trade => trade.Symbol == Crypto).Validate(Crypto);
        trades.Items.Where(trade => trade.Symbol != Crypto).Validate(Other);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbol);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Crypto, trades.Symbol);

        trades.Items.Validate(Crypto);
    }
}
