using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var today = DateTime.Today;
        var exchange = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/quotes", new JsonMultiQuotesPage<JsonHistoricalCryptoQuote>
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalCryptoQuote>?>
            {
                { Crypto, createQuotesList(exchange) },
                { Other, createQuotesList(exchange) }
            }
        });

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(new[] { Crypto, Other }, today, today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Crypto], exchange, Crypto);
        validateQuotesList(quotes.Items[Other], exchange, Other);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v1beta1/crypto/**/quotes", new JsonQuotesPage<JsonHistoricalCryptoQuote>
        {
            ItemsList = createQuotesList(condition),
            Symbol = Crypto
        });

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, today, today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Crypto, quotes.Symbol);

        validateQuotesList(quotes.Items, condition, Crypto);
    }

    private static List<JsonHistoricalCryptoQuote> createQuotesList(String exchange) => 
        new () { createQuote(exchange), createQuote(exchange) };

    private static JsonHistoricalCryptoQuote createQuote(String exchange) =>
        new () { AskExchange = exchange };

    private static void validateQuotesList(
        IEnumerable<IQuote> trades,
        String exchange,
        String symbol)
    {
        foreach (var trade in trades)
        {
            validateQuote(trade, symbol, exchange);
        }
    }

    private static void validateQuote(
        IQuote trade,
        String symbol,
        String exchange)
    {
        Assert.Empty(trade.Conditions);
        Assert.Equal(symbol, trade.Symbol);
        Assert.Equal(exchange, trade.AskExchange);
        Assert.Equal(exchange, trade.BidExchange);
    }
}
