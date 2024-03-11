namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetHistoricalBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalOptionBarsRequest(_symbols, Yesterday, Today, BarTimeFrame.Hour));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        foreach (var symbol in _symbols)
        {
            bars.Items[symbol].Validate(symbol);
        }
    }

    [Fact]
    public async Task GetHistoricalBarsAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, [ Symbol ]);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalOptionBarsRequest(Symbol, BarTimeFrame.Hour, _timeInterval));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        bars.Items[Symbol].Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, [ Symbol ]);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalOptionBarsRequest(Symbol, Yesterday, Today, BarTimeFrame.Hour));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Symbol, bars.Symbol);

        bars.Items.Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalOptionBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(String.Empty, bars.Symbol);

        foreach (var symbol in _symbols)
        {
            bars.Items.Where(bar => bar.Symbol == symbol).Validate(symbol);
        }
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, [ Symbol ]);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalOptionBarsRequest(Symbol, BarTimeFrame.Hour));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Symbol, bars.Symbol);

        bars.Items.Validate(Symbol);
    }
}
