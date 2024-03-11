namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalOptionTradesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        foreach (var symbol in _symbols)
        {
            trades.Items[symbol].Validate(symbol);
        }
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, [ Symbol ]);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalOptionTradesRequest(Symbol, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        trades.Items[Symbol].Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, [ Symbol ]);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalOptionTradesRequest(Symbol, Yesterday, Today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Symbol, trades.Symbol);

        trades.Items.Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalOptionTradesRequest(_symbols, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        foreach (var symbol in _symbols)
        {
            trades.Items.Where(bar => bar.Symbol == symbol).Validate(symbol);
        }
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiTradesPageExpectation(PathPrefix, [ Symbol ]);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalOptionTradesRequest(Symbol));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Symbol, trades.Symbol);

        trades.Items.Validate(Symbol);
    }
}
