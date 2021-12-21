using System;

namespace Alpaca.Markets
{
    internal sealed class AlpacaDataStreamingClient :
        DataStreamingClientBase<AlpacaDataStreamingClientConfiguration>,
        IAlpacaDataStreamingClient
    {
        public AlpacaDataStreamingClient(
            AlpacaDataStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
        }

        public IAlpacaDataSubscription<ITrade> GetTradeSubscription(
            String symbol) =>
            GetSubscription<ITrade, JsonRealTimeTrade>(TradesChannel, symbol);

        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol) =>
            GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol);

        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
            GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, WildcardSymbolString);

        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            String symbol) =>
            GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, symbol);

        public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            String symbol) =>
            GetSubscription<IBar, JsonRealTimeBar>(DailyBarsChannel, symbol);

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
}
