namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiQuotesPageExpectation(mock);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, _yesterday, _today)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Crypto], Crypto);
        validateQuotesList(quotes.Items[Other], Other);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleQuotesPageExpectation(mock);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, _timeInterval)
                .WithExchanges(CryptoExchange.Cbse));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        validateQuotesList(quotes.Items[Crypto], Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addSingleQuotesPageExpectation(mock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, _yesterday, _today)
                .WithExchanges(_exchangesList));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Crypto, quotes.Symbol);

        validateQuotesList(quotes.Items, Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addMultiQuotesPageExpectation(mock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, _timeInterval)
                .WithExchanges(_exchangesList));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        validateQuotesList(quotes.Items.Where(_ => _.Symbol == Crypto), Crypto);
        validateQuotesList(quotes.Items.Where(_ => _.Symbol != Crypto), Other);
    }

    private static void addMultiQuotesPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/quotes", new JsonMultiQuotesPage<JsonHistoricalCryptoQuote>
        {
            ItemsDictionary = new Dictionary<String, List<JsonHistoricalCryptoQuote>?>
            {
                { Crypto, createQuotesList() },
                { Other, createQuotesList() }
            }
        });

    private static void addSingleQuotesPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock) =>
        mock.AddGet("/v1beta1/crypto/**/quotes", new JsonQuotesPage<JsonHistoricalCryptoQuote>
        {
            ItemsList = createQuotesList(),
            Symbol = Crypto
        });

    private static List<JsonHistoricalCryptoQuote> createQuotesList() => 
        new () { createQuote(), createQuote() };

    private static JsonHistoricalCryptoQuote createQuote() =>
        new () { AskExchange = _exchange };

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
        Assert.Empty(quote.Conditions);
        Assert.Equal(symbol, quote.Symbol);
        Assert.Equal(_exchange, quote.AskExchange);
        Assert.Equal(_exchange, quote.BidExchange);

        Assert.Equal(String.Empty, quote.Tape);
    }
}
