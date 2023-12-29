namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    private const Decimal Volume = 123_456M;

    private const UInt64 TradeCount = 100UL;

    [Fact]
    public async Task GetTopMarketMoversAsyncWorks()
    {
        const Int32 numberOfLosersAndGainers = 5;

        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v1beta1/screener/stocks/movers", new JObject(
            new JProperty("gainers", new JArray(Enumerable.Repeat(
                Stock.CreateStockMover(), numberOfLosersAndGainers))),
            new JProperty("losers", new JArray(Enumerable.Repeat(
                Stock.CreateStockMover(), numberOfLosersAndGainers)))));

        var movers = await mock.Client.GetTopMarketMoversAsync(numberOfLosersAndGainers);

        Assert.NotNull(movers);
        Assert.Equal(numberOfLosersAndGainers, movers.Losers.Count);
        Assert.Equal(numberOfLosersAndGainers, movers.Gainers.Count);

        movers.Gainers.Validate(Stock);
        movers.Losers.Validate(Stock);
    }

    [Fact]
    public async Task ListMostActiveStocksByVolumeAsyncWorks()
    {
        const Int32 numberOfTopMostActiveStocks = 5;

        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v1beta1/screener/stocks/most-actives", new JObject(
            new JProperty("most_actives", new JArray(Enumerable.Repeat(
                createActiveStock(), numberOfTopMostActiveStocks)))));

        var activeStocksList = await mock.Client.ListMostActiveStocksByVolumeAsync(numberOfTopMostActiveStocks);

        Assert.NotNull(activeStocksList);
        Assert.Equal(numberOfTopMostActiveStocks, activeStocksList.Count);
        Assert.All(activeStocksList, validateActiveStock);
    }

    [Fact]
    public async Task ListMostActiveStocksByTradeCountAsyncWorks()
    {
        const Int32 numberOfTopMostActiveStocks = 5;

        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v1beta1/screener/stocks/most-actives", new JObject(
            new JProperty("most_actives", new JArray(Enumerable.Repeat(
                createActiveStock(), numberOfTopMostActiveStocks)))));

        var activeStocksList = await mock.Client.ListMostActiveStocksByTradeCountAsync(numberOfTopMostActiveStocks);

        Assert.NotNull(activeStocksList);
        Assert.Equal(numberOfTopMostActiveStocks, activeStocksList.Count);
        Assert.All(activeStocksList, validateActiveStock);
    }

    private static JObject createActiveStock() =>
        new(
            new JProperty("trade_count", TradeCount),
            new JProperty("volume", Volume),
            new JProperty("symbol", Stock));

    private static void validateActiveStock(
        IActiveStock activeStock)
    {
        Assert.Equal(TradeCount, activeStock.TradeCount);
        Assert.Equal(Volume, activeStock.Volume);
        Assert.Equal(Stock, activeStock.Symbol);
    }
}
