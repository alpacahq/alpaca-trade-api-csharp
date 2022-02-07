using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiTradesPageExpectation(mock);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, _yesterday, _today)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Crypto], Crypto);
        validateTradesList(trades.Items[Other], Other);
    }

    [Fact]
    public async Task GetHistoricalTradesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleTradesPageExpectation(mock);

        var trades = await mock.Client.GetHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, _timeInterval)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);

        validateTradesList(trades.Items[Crypto], Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleTradesPageExpectation(mock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(Crypto, _yesterday, _today)
                .WithExchanges(_exchangesList));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(Crypto, trades.Symbol);

        validateTradesList(trades.Items, Crypto);
    }

    [Fact]
    public async Task ListHistoricalTradesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiTradesPageExpectation(mock);

        var trades = await mock.Client.ListHistoricalTradesAsync(
            new HistoricalCryptoTradesRequest(_symbols, _timeInterval)
                .WithExchanges(_exchangesList));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades.Items);
        Assert.Equal(String.Empty, trades.Symbol);

        validateTradesList(trades.Items.Where(_ => _.Symbol == Crypto), Crypto);
        validateTradesList(trades.Items.Where(_ => _.Symbol != Crypto), Other);
    }

    private static void addSingleTradesPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/**/trades", new JsonTradesPage
        {
            ItemsList = createTradesList(),
            Symbol = Crypto
        });

    private static void addMultiTradesPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/trades", new JsonMultiTradesPage
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalTrade>?>
            {
                { Crypto, createTradesList() },
                { Other, createTradesList() }
            }
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
