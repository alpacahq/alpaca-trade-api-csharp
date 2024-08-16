
namespace Alpaca.Markets;

internal sealed class AlpacaOptionsStreamingClient :
    DataStreamingClientBase<AlpacaOptionsStreamingClientConfiguration>,
    IStreamingDataClient
{
    protected override bool IsMessagePack => true;
    internal AlpacaOptionsStreamingClient(
        AlpacaOptionsStreamingClientConfiguration configuration)
        : base(configuration.EnsureNotNull())
    {
    }

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription() =>
        GetSubscription<IQuote, JsonRealTimeCryptoQuote>(QuotesChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol) =>
        GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol.EnsureNotNull());
}
