namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetLatestQuoteAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var exchange = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/**/quotes/latest", 
            new JsonLatestQuote<JsonHistoricalCryptoQuote>
            {
                Nested = new JsonHistoricalCryptoQuote
                {
                    TimestampUtc = DateTime.UtcNow,
                    AskExchange = exchange,
                    AskPrice = 100M,
                    BidPrice = 200M,
                    AskSize = 42M,
                    BidSize = 42M
                },
                Symbol = Crypto
            });

        var quote = await mock.Client.GetLatestQuoteAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Cbse));

        Assert.Equal(Crypto, quote.Symbol);

        Assert.Equal(exchange, quote.AskExchange);
        Assert.Equal(exchange, quote.BidExchange);
        Assert.Equal(String.Empty, quote.Tape);
        Assert.True(quote.TimestampUtc <= DateTime.UtcNow);

        Assert.Equal(100M, quote.AskPrice);
        Assert.Equal(200M, quote.BidPrice);
        Assert.Equal(42M, quote.AskSize);
        Assert.Equal(42M, quote.BidSize);

        Assert.NotNull(quote.Conditions);
        Assert.Empty(quote.Conditions);
    }

    [Fact]
    public async Task GetLatestTradeAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var exchange = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/**/trades/latest",
            new JsonLatestTrade
            {
                Nested = new JsonHistoricalTrade
                {
                    TimestampUtc = DateTime.UtcNow,
                    Exchange = exchange,
                    TradeId = 12345UL,
                    Price = 100M,
                    Size = 42M
                },
                Symbol = Crypto
            });

        var trade = await mock.Client.GetLatestTradeAsync(
            new LatestDataRequest(Crypto, CryptoExchange.Ersx));

        Assert.Equal(Crypto, trade.Symbol);

        Assert.Equal(exchange, trade.Exchange);
        Assert.Equal(String.Empty, trade.Tape);
        Assert.True(trade.TimestampUtc <= DateTime.UtcNow);

        Assert.Equal(TakerSide.Unknown, trade.TakerSide);
        Assert.Equal(12345UL, trade.TradeId);
        Assert.Equal(100M, trade.Price);
        Assert.Equal(42M, trade.Size);

        Assert.NotNull(trade.Conditions);
        Assert.Empty(trade.Conditions);
    }
}
