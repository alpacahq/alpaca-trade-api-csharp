namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefixV2, _symbols);

#pragma warning disable CS0618
        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, Yesterday, Today));
#pragma warning restore CS0618

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Crypto].Validate(Crypto);
        quotes.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefixV2, _symbol);

#pragma warning disable CS0618
        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, _timeInterval));
#pragma warning restore CS0618

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Crypto].Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefixV2, _symbol);

#pragma warning disable CS0618
        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, Yesterday, Today));
#pragma warning restore CS0618

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Crypto, quotes.Symbol);

        quotes.Items.Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefixV2, _symbols);

#pragma warning disable CS0618
        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, _timeInterval));
#pragma warning restore CS0618

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        quotes.Items.Where(_ => _.Symbol == Crypto).Validate(Crypto);
        quotes.Items.Where(_ => _.Symbol != Crypto).Validate( Other);
    }
}
