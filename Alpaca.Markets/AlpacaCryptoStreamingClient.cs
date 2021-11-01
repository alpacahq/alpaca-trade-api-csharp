using System;

namespace Alpaca.Markets
{
    internal sealed class AlpacaCryptoStreamingClient :
        DataStreamingClientBase<AlpacaCryptoStreamingClientConfiguration>, 
        IAlpacaCryptoStreamingClient
    {
        public AlpacaCryptoStreamingClient(
            AlpacaCryptoStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
        }

        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol) =>
            GetSubscription<IQuote, JsonRealTimeCryptoQuote>(QuotesChannel, symbol);
    }
}
