using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v2/stocks/trades", new JsonMultiTradesPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalTrade>?>
            {
                { Stock, createTradesList(condition) },
                { Other, createTradesList(condition) }
            }
        });

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalTradesRequest(new [] { Stock, Other }, today, today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Stock], condition, Stock);
        validateTradesList(trades.Items[Other], condition, Other);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v2/stocks/**/trades", new JsonTradesPage
        {
            ItemsList = createTradesList(condition),
            Symbol = Stock
        });

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalTradesRequest(Stock, today, today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Stock, trades.Symbol);

        validateTradesList(trades.Items, condition, Stock);
    }

    private static List<JsonHistoricalTrade> createTradesList(String condition) => 
        new () { createTrade(condition), createTrade(condition) };

    private static JsonHistoricalTrade createTrade(String condition) =>
        new () { ConditionsList = { condition } };

    private static void validateTradesList(
        IEnumerable<ITrade> trades,
        String condition,
        String symbol)
    {
        foreach (var trade in trades)
        {
            validateTrade(trade, symbol, condition);
        }
    }

    private static void validateTrade(
        ITrade trade,
        String symbol,
        String condition)
    {
        Assert.NotEmpty(trade.Conditions);
        Assert.Equal(symbol, trade.Symbol);
        Assert.Equal(condition, trade.Conditions.Single());
    }
}
