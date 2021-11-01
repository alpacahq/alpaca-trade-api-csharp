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

        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol) =>
            GetSubscription<IQuote, JsonRealTimeQuote>(QuotesChannel, symbol);

        public IAlpacaDataSubscription<IStatus> GetStatusSubscription(
            String symbol) =>
            GetSubscription<IStatus, JsonTradingStatus>(StatusesChannel, symbol);

        public IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
            String symbol) =>
            GetSubscription<ILimitUpLimitDown, JsonLimitUpLimitDown>(LimitUpDownChannel, symbol);
    }
}
