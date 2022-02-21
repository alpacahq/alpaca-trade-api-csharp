namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, _yesterday, _today)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Crypto].Validate(Crypto);
        trades.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddSingleTradesPageExpectation(PathPrefix, Crypto);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, _timeInterval)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Crypto].Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddSingleTradesPageExpectation(PathPrefix, Crypto);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, _yesterday, _today)
                .WithExchanges(_exchangesList));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Crypto, trades.Symbol);

        trades.Items.Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, _timeInterval)
                .WithExchanges(_exchangesList));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        trades.Items.Where(_ => _.Symbol == Crypto).Validate(Crypto);
        trades.Items.Where(_ => _.Symbol != Crypto).Validate(Other);
    }
}
