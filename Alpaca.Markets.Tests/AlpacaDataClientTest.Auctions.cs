namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetHistoricalAuctionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiAuctionsPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.GetHistoricalAuctionsAsync(
            new HistoricalAuctionsRequest(_symbols, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Stock].Validate(Stock);
        quotes.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalAuctionsAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleAuctionsPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.GetHistoricalAuctionsAsync(
            new HistoricalAuctionsRequest(Stock, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Stock].Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalAuctionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleAuctionsPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.ListHistoricalAuctionsAsync(
            new HistoricalAuctionsRequest(Stock, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        quotes.Items.Validate(Stock);
    }

    [Fact]
    public async Task ListHistoricalAuctionsAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddMultiAuctionsPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListHistoricalAuctionsAsync(
            new HistoricalAuctionsRequest(_symbols, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        quotes.Items.Where(_ => _.Symbol == Stock).Validate(Stock);
        quotes.Items.Where(_ => _.Symbol != Stock).Validate(Other);
    }

    [Fact]
    public async Task ListHistoricalAuctionsAsyncWithoutIntervalWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddSingleAuctionsPageExpectation(PathPrefix, Stock);

        var quotes = await mock.Client.ListHistoricalAuctionsAsync(
            new HistoricalAuctionsRequest(Stock));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Stock, quotes.Symbol);

        quotes.Items.Validate(Stock);
    }
}
