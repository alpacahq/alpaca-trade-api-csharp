using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v2/stocks/quotes", new JsonMultiQuotesPage<JsonHistoricalQuote>
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalQuote>?>
            {
                { Stock, createQuotesList(condition) },
                { Other, createQuotesList(condition) }
            }
        });

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalQuotesRequest(new[] { Stock, Other }, today, today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Stock], condition, Stock);
        validateQuotesList(quotes.Items[Other], condition, Other);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        var today = DateTime.Today;
        var condition = Guid.NewGuid().ToString("D");

        mock.AddGet("/v2/stocks/**/quotes", new JsonQuotesPage<JsonHistoricalQuote>
        {
            ItemsList = createQuotesList(condition),
            Symbol = Stock
        });

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock, today, today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        validateQuotesList(quotes.Items, condition, Stock);
    }

    private static List<JsonHistoricalQuote> createQuotesList(String condition) => 
        new () { createQuote(condition), createQuote(condition) };

    private static JsonHistoricalQuote createQuote(String condition) =>
        new () { ConditionsList = { condition } };

    private static void validateQuotesList(
        IEnumerable<IQuote> trades,
        String condition,
        String symbol)
    {
        foreach (var trade in trades)
        {
            validateQuote(trade, symbol, condition);
        }
    }

    private static void validateQuote(
        IQuote trade,
        String symbol,
        String condition)
    {
        Assert.NotEmpty(trade.Conditions);
        Assert.Equal(symbol, trade.Symbol);
        Assert.Equal(condition, trade.Conditions.Single());
    }
}
