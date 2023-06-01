namespace Alpaca.Markets;

internal sealed class AlpacaDataStreamingClient :
    DataStreamingClientBase<AlpacaDataStreamingClientConfiguration>,
    IAlpacaDataStreamingClient
{
    internal AlpacaDataStreamingClient(
        AlpacaDataStreamingClientConfiguration configuration)
        : base(configuration.EnsureNotNull())
    {
    }

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription() =>
        GetSubscription<IQuote, JsonRealTimeCryptoQuote>(QuotesChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol) =>
        GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IStatus> GetStatusSubscription(
        String symbol) =>
        GetSubscription<IStatus, JsonTradingStatus>(StatusesChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<ITrade> GetCancellationSubscription(
        String symbol) =>
        GetSubscription<ITrade, JsonRealTimeTrade>(CancellationsChannel, symbol.EnsureNotNull(),
            GetTradeSubscription(symbol.EnsureNotNull()));

    public IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(
        String symbol) =>
        GetSubscription<ICorrection, JsonCorrection>(CorrectionsChannel, symbol.EnsureNotNull(),
            GetTradeSubscription(symbol.EnsureNotNull()));
        
    public IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
        String symbol) =>
        GetSubscription<ILimitUpLimitDown, JsonLimitUpLimitDown>(LimitUpDownChannel, symbol.EnsureNotNull());
}
