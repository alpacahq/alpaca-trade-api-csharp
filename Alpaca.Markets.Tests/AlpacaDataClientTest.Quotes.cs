namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalQuotesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Stock].Validate(Stock);
        quotes.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleQuotesPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Stock].Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleQuotesPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        quotes.Items.Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(_symbols, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        quotes.Items.Where(quote => quote.Symbol == Stock).Validate(Stock);
        quotes.Items.Where(quote => quote.Symbol != Stock).Validate(Other);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleQuotesPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalQuotesRequest(Stock));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        quotes.Items.Validate(Stock);
    }
}
