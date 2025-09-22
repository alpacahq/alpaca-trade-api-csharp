namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalOptionQuotesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Symbol].Validate(Symbol);
        quotes.Items[_symbols[1]].Validate(_symbols[1]);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, [ Symbol ]);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalOptionQuotesRequest(Symbol, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Symbol].Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, [ Symbol ]);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalOptionQuotesRequest(Symbol, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Symbol, quotes.Symbol);

        quotes.Items.Validate(Symbol);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalOptionQuotesRequest(_symbols, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        quotes.Items.Where(quote => quote.Symbol == Symbol).Validate(Symbol);
        quotes.Items.Where(quote => quote.Symbol != Symbol).Validate(_symbols[1]);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, [ Symbol ]);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalOptionQuotesRequest(Symbol));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Symbol, quotes.Symbol);

        quotes.Items.Validate(Symbol);
    }
}