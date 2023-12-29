namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalTradesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Stock].Validate(Stock);
        trades.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleTradesPageExpectation(PathPrefix, Stock);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Stock].Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleTradesPageExpectation(PathPrefix, Stock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Stock, trades.Symbol);

        trades.Items.Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(_symbols, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        trades.Items.Where(trade => trade.Symbol == Stock).Validate(Stock);
        trades.Items.Where(trade => trade.Symbol != Stock).Validate(Other);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleTradesPageExpectation(PathPrefix, Stock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Stock, trades.Symbol);

        trades.Items.Validate(Stock);
    }
}
