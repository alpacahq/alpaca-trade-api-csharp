namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalBarsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiBarsPageExpectation(mock);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalCryptoBarsRequest(_symbols, _yesterday, _today, BarTimeFrame.Hour)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        validateBarsList(bars.Items[Crypto], Crypto);
        validateBarsList(bars.Items[Other], Other);
    }

    [Fact]
    public async Task GetHistoricalBarsAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleBarsPageExpectation(mock);

        var bars = await mock.Client.GetHistoricalBarsAsync(
            new HistoricalCryptoBarsRequest(Crypto, BarTimeFrame.Hour, _timeInterval));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);

        validateBarsList(bars.Items[Crypto], Crypto);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleBarsPageExpectation(mock);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalCryptoBarsRequest(Crypto, _yesterday, _today, BarTimeFrame.Hour)
                .WithExchanges(_exchangesList));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(Crypto, bars.Symbol);

        validateBarsList(bars.Items, Crypto);
    }

    [Fact]
    public async Task ListHistoricalBarsAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiBarsPageExpectation(mock);

        var bars = await mock.Client.ListHistoricalBarsAsync(
            new HistoricalCryptoBarsRequest(_symbols, _timeInterval, BarTimeFrame.Hour)
                .WithExchanges(_exchangesList));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars.Items);
        Assert.Equal(String.Empty, bars.Symbol);

        validateBarsList(bars.Items.Where(_ => _.Symbol == Crypto), Crypto);
        validateBarsList(bars.Items.Where(_ => _.Symbol != Crypto), Other);
    }

    private static void addMultiBarsPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/bars", new JsonMultiBarsPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalBar>?>
            {
                { Crypto, createBarsList() },
                { Other, createBarsList() }
            }
        });

    private static void addSingleBarsPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/**/bars", new JsonBarsPage
        {
            ItemsList = createBarsList(),
            Symbol = Crypto
        });

    private static List<JsonHistoricalBar> createBarsList() =>
        new() { createBar(), createBar() };

    private static JsonHistoricalBar createBar() =>
        new()
        {
            TimeUtc = DateTime.UtcNow,
            TradeCount = 100,
            Volume = 1000M,
            Close = 110M,
            Open = 100M,
            High = 120M,
            Low = 90M
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
