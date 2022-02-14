namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addMultiTradesPageExpectation(mock);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalTradesRequest(_symbols, _yesterday, _today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Stock], Stock);
        validateTradesList(trades.Items[Other], Other);
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addSingleTradesPageExpectation(mock);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Stock], Stock);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addSingleTradesPageExpectation(mock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock, _yesterday, _today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Stock, trades.Symbol);

        validateTradesList(trades.Items, Stock);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addMultiTradesPageExpectation(mock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(_symbols, _timeInterval));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        validateTradesList(trades.Items.Where(_ => _.Symbol == Stock), Stock);
        validateTradesList(trades.Items.Where(_ => _.Symbol != Stock), Other);
    }

    private static void addMultiTradesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock) =>
        mock.AddGet("/v2/stocks/trades", new JsonMultiTradesPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalTrade>?>
            {
                { Stock, createTradesList() },
                { Other, createTradesList() }
            }
        });

    private static void addSingleTradesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock) =>
        mock.AddGet("/v2/stocks/**/trades", new JsonTradesPage
        {
            ItemsList = createTradesList(),
            Symbol = Stock
        });

    private static List<JsonHistoricalTrade> createTradesList() => 
        new () { createTrade(), createTrade() };

    private static JsonHistoricalTrade createTrade() =>
        new () { ConditionsList = { _condition } };

    private static void validateTradesList(
        IEnumerable<ITrade> trades,
        String symbol)
    {
        foreach (var trade in trades)
        {
            validateTrade(trade, symbol);
        }
    }

    private static void validateTrade(
        ITrade trade,
        String symbol)
    {
        Assert.NotEmpty(trade.Conditions);
        Assert.Equal(symbol, trade.Symbol);
        Assert.Equal(_condition, trade.Conditions.Single());
    }
}
