using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddGet("/v1beta1/crypto/**/snapshot", new JsonCryptoSnapshot
        {
            JsonPreviousDailyBar = new JsonHistoricalBar(),
            JsonCurrentDailyBar = new JsonHistoricalBar(),
            JsonQuote = new JsonHistoricalCryptoQuote(),
            JsonMinuteBar = new JsonHistoricalBar(),
            JsonTrade = new JsonHistoricalTrade(),
            Symbol = Crypto
        });

        var snapshot = await mock.Client.GetSnapshotAsync(
            new SnapshotDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.NotNull(snapshot.Quote);
        Assert.NotNull(snapshot.Trade);
        Assert.NotNull(snapshot.MinuteBar);
        Assert.NotNull(snapshot.CurrentDailyBar);
        Assert.NotNull(snapshot.PreviousDailyBar);

        Assert.NotNull(snapshot.Quote!.Conditions);
        Assert.Empty(snapshot.Quote!.Conditions);

        Assert.Equal(Crypto, snapshot.Symbol);
        Assert.Equal(Crypto, snapshot.Quote!.Symbol);
        Assert.Equal(Crypto, snapshot.Trade!.Symbol);
        Assert.Equal(Crypto, snapshot.MinuteBar!.Symbol);
        Assert.Equal(Crypto, snapshot.CurrentDailyBar!.Symbol);
        Assert.Equal(Crypto, snapshot.PreviousDailyBar!.Symbol);
    }

    [Fact]
    public async Task GetLatestBestBidOfferAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/**/xbbo/latest", new JsonLatestBestBidOffer
        {
            Nested = new JsonHistoricalQuote
            {
                ConditionsList = new List<String> { condition }
            },
            Symbol = Crypto
        });

        var bbo = await mock.Client.GetLatestBestBidOfferAsync(
            new LatestBestBidOfferRequest(Crypto, new [] { CryptoExchange.Ersx, CryptoExchange.Gnss }));

        Assert.Equal(Crypto, bbo.Symbol);
        Assert.NotEmpty(bbo.Conditions);
    }
}
