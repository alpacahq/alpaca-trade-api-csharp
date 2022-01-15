namespace Alpaca.Markets
{
    internal sealed class AlpacaNewsStreamingClient :
        DataStreamingClientBase<AlpacaNewsStreamingClientConfiguration>, 
        IAlpacaNewsStreamingClient
    {
        public AlpacaNewsStreamingClient(
            AlpacaNewsStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
        }

        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription() => 
            GetSubscription<INewsArticle, JsonNewsArticle>(NewsChannel, WildcardSymbolString);

        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(
            String symbol) => 
            GetSubscription<INewsArticle, JsonNewsArticle>(NewsChannel, symbol);

        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol) =>
            throw new NotImplementedException("This method will be removed soon.");
    }
}
