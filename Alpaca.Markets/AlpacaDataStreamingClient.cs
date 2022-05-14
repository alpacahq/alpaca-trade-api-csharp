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

    public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol) =>
        GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol);

    public IAlpacaDataSubscription<IStatus> GetStatusSubscription(
        String symbol) =>
        GetSubscription<IStatus, JsonTradingStatus>(StatusesChannel, symbol);

    public IAlpacaDataSubscription<ITrade> GetCancellationSubscription(
        String symbol) =>
        GetSubscription<ITrade, JsonRealTimeTrade>(CancellationsChannel, symbol,
            GetTradeSubscription(symbol));

    public IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(
        String symbol) =>
        GetSubscription<ICorrection, JsonCorrection>(CorrectionsChannel, symbol,
            GetTradeSubscription(symbol));
        
    public IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
        String symbol) =>
        GetSubscription<ILimitUpLimitDown, JsonLimitUpLimitDown>(LimitUpDownChannel, symbol);
}
