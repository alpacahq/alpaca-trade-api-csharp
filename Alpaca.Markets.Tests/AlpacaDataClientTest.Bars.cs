using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalBarsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;

        mock.AddGet("/v2/stocks/bars", new JsonMultiBarsPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalBar>?>
            {
                { Stock, createBarsList() },
                { Other, createBarsList() }
            }
        });

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalBarsRequest(new [] { Stock, Other }, 
                today, today, BarTimeFrame.Hour));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        validateBarsList(bars.Items[Stock], Stock);
        validateBarsList(bars.Items[Other], Other);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;

        mock.AddGet("/v2/stocks/**/bars", new JsonBarsPage
        {
            ItemsList = createBarsList(),
            Symbol = Stock
        });

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalBarsRequest(Stock, today, today, BarTimeFrame.Minute));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Stock, bars.Symbol);

        validateBarsList(bars.Items, Stock);
    }

    private static List<JsonHistoricalBar> createBarsList() => 
        new () { createBar(), createBar() };

    private static JsonHistoricalBar createBar() =>
        new ()
        {
            TimeUtc = DateTime.UtcNow,
            TradeCount = 100,
            Volume = 1000M,
            Close = 110M,
            Open = 100M,
            High = 120M,
            Low = 90M,
        };

    private static void validateBarsList(
        IEnumerable<IBar> bars,
        String symbol)
    {
        foreach (var trade in bars)
        {
            validateBar(trade, symbol);
        }
    }

    private static void validateBar(
        IBar bar,
        String symbol)
    {
        Assert.Equal(symbol, bar.Symbol);
        
        Assert.InRange(bar.Close, bar.Low, bar.High);
        Assert.InRange(bar.Open, bar.Low, bar.High);

        Assert.True(bar.TimeUtc <= DateTime.UtcNow);
        Assert.True(bar.TradeCount != 0);
        Assert.True(bar.Volume != 0M);
    }
}
