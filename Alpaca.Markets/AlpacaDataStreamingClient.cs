using System;

namespace Alpaca.Markets
{
    internal sealed class AlpacaDataStreamingClient :
        DataStreamingClientBase<AlpacaDataStreamingClientConfiguration>, 
        IAlpacaDataStreamingClient
    {
        // Available Alpaca data streaming message types

        private const String StatusesChannel = "s";

        private const String DailyBarsChannel = "d";

        private const String LimitUpDownChannel = "l";

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

        public IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
            String symbol) =>
            GetSubscription<ILimitUpLimitDown, JsonLimitUpLimitDown>(LimitUpDownChannel, symbol);
    }
}
