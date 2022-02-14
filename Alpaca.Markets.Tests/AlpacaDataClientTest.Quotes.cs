namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addMultiQuotesPageExpectation(mock);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalQuotesRequest(_symbols, _yesterday, _today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Stock], Stock);
        validateQuotesList(quotes.Items[Other], Other);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addSingleQuotesPageExpectation(mock);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Stock], Stock);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addSingleQuotesPageExpectation(mock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock, _yesterday, _today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        validateQuotesList(quotes.Items, Stock);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        addMultiQuotesPageExpectation(mock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(_symbols, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        validateQuotesList(quotes.Items.Where(_ => _.Symbol == Stock), Stock);
        validateQuotesList(quotes.Items.Where(_ => _.Symbol != Stock), Other);
    }

    private static void addMultiQuotesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock) =>
        mock.AddGet("/v2/stocks/quotes", new JsonMultiQuotesPage<JsonHistoricalQuote>
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalQuote>?>
            {
                { Stock, createQuotesList() },
                { Other, createQuotesList() }
            }
        });

    private static void addSingleQuotesPageExpectation(
        MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> mock) =>
        mock.AddGet("/v2/stocks/**/quotes", new JsonQuotesPage<JsonHistoricalQuote>
        {
            ItemsList = createQuotesList(),
            Symbol = Stock
        });

    private static List<JsonHistoricalQuote> createQuotesList() => 
        new () { createQuote(), createQuote() };

    private static JsonHistoricalQuote createQuote() =>
        new () { ConditionsList = { _condition } };

    private static void validateQuotesList(
        IEnumerable<IQuote> quotes,
        String symbol)
    {
        foreach (var quote in quotes)
        {
            validateQuote(quote, symbol);
        }
    }

    private static void validateQuote(
        IQuote quote,
        String symbol)
    {
        Assert.NotEmpty(quote.Conditions);
        Assert.Equal(symbol, quote.Symbol);
        Assert.Equal(_condition, quote.Conditions.Single());
    }
}
