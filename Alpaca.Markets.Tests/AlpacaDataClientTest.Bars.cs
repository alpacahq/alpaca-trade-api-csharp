namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalBarsRequest(_symbols, Yesterday, Today, BarTimeFrame.Hour)
            {
                Adjustment = Adjustment.SplitsAndDividends
            });

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        bars.Items[Stock].Validate(Stock);
        bars.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalBarsAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleBarsPageExpectation(PathPrefix, Stock);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalBarsRequest(Stock, BarTimeFrame.Hour, _timeInterval)
            {
                Adjustment = Adjustment.DividendsOnly
            });

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        bars.Items[Stock].Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleBarsPageExpectation(PathPrefix, Stock);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(Stock, Yesterday, Today, BarTimeFrame.Hour)
            {
                Adjustment = Adjustment.SplitsOnly
            });

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Stock, bars.Symbol);

        bars.Items.Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiBarsPageExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)
            {
                Adjustment = Adjustment.Nothing
            });

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(String.Empty, bars.Symbol);

        bars.Items.Where(bar => bar.Symbol == Stock).Validate(Stock);
        bars.Items.Where(bar => bar.Symbol != Stock).Validate(Other);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleBarsPageExpectation(PathPrefix, Stock);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(Stock, BarTimeFrame.Hour)
            {
                Adjustment = Adjustment.SplitsOnly
            });

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Stock, bars.Symbol);

        bars.Items.Validate(Stock);
    }
}
