﻿namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaCryptoDataClientTest
{
    [Fact]
    public async Task GetHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Crypto].Validate(Crypto);
        quotes.Items[Other].Validate(Other);
    }

    [Fact]
    public async Task GetHistoricalQuotesAsyncForSingleWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbol);

        var quotes = await mock.Client.GetHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);

        quotes.Items[Crypto].Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbol);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto, Yesterday, Today));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Crypto, quotes.Symbol);

        quotes.Items.Validate(Crypto);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncForManyWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(_symbols, _timeInterval));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(String.Empty, quotes.Symbol);

        quotes.Items.Where(quote => quote.Symbol == Crypto).Validate(Crypto);
        quotes.Items.Where(quote => quote.Symbol != Crypto).Validate( Other);
    }

    [Fact]
    public async Task ListHistoricalQuotesAsyncWithoutIntervalWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaCryptoDataClientMock();

        mock.AddMultiQuotesPageExpectation(PathPrefix, _symbol);

        var quotes = await mock.Client.ListHistoricalQuotesAsync(
            new HistoricalCryptoQuotesRequest(Crypto));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes.Items);
        Assert.Equal(Crypto, quotes.Symbol);

        quotes.Items.Validate(Crypto);
    }
}
