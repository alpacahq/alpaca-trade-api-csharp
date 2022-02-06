using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/trades", new JsonMultiTradesPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalTrade>?>
            {
                { Crypto, createTradesList(condition) },
                { Other, createTradesList(condition) }
            }
        });

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(new [] { Crypto, Other }, today, today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Crypto], condition, Crypto);
        validateTradesList(trades.Items[Other], condition, Other);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/**/trades", new JsonTradesPage
        {
            ItemsList = createTradesList(condition),
            Symbol = Crypto
        });

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, today, today));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Crypto, trades.Symbol);

        validateTradesList(trades.Items, condition, Crypto);
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
