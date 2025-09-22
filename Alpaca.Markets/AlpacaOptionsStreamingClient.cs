namespace Alpaca.Markets;

internal sealed class AlpacaOptionsStreamingClient :
    DataStreamingClientBase<AlpacaOptionsStreamingClientConfiguration>,
    IAlpacaOptionsStreamingClient
{
    internal AlpacaOptionsStreamingClient(
        AlpacaOptionsStreamingClientConfiguration configuration)
        : base(configuration.EnsureNotNull())
    {
    }

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription() =>
        GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol) =>
        GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol.EnsureNotNull());
}