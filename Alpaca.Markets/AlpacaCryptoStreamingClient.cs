﻿namespace Alpaca.Markets;

internal sealed class AlpacaCryptoStreamingClient :
    DataStreamingClientBase<AlpacaCryptoStreamingClientConfiguration>,
    IAlpacaCryptoStreamingClient
{
    internal AlpacaCryptoStreamingClient(
        AlpacaCryptoStreamingClientConfiguration configuration)
        : base(configuration.EnsureNotNull())
    {
    }

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol) =>
        GetSubscription<IQuote, JsonRealTimeCryptoQuote>(QuotesChannel, symbol);

    public IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(
        String symbol) => 
        GetSubscription<IOrderBook, JsonOrderBook>(OrderBooksChannel, symbol);
}
