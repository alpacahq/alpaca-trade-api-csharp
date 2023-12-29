namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetTopMarketMoversAsyncWorks()
    {
        const Int32 numberOfLosersAndGainers = 5;

        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddGet("/v1beta1/screener/crypto/movers", new JObject(
            new JProperty("gainers", new JArray(Enumerable.Repeat(
                Crypto.CreateStockMover(), numberOfLosersAndGainers))),
            new JProperty("losers", new JArray(Enumerable.Repeat(
                Crypto.CreateStockMover(), numberOfLosersAndGainers)))));

        var movers = await mock.Client.GetTopMarketMoversAsync(numberOfLosersAndGainers);

        Assert.NotNull(movers);
        Assert.Equal(numberOfLosersAndGainers, movers.Losers.Count);
        Assert.Equal(numberOfLosersAndGainers, movers.Gainers.Count);

        movers.Gainers.Validate(Crypto);
        movers.Losers.Validate(Crypto);
    }
}
