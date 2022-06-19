namespace Alpaca.Markets;

internal sealed class AlpacaNewsStreamingClient :
    DataStreamingClientBase<AlpacaNewsStreamingClientConfiguration>, 
    IAlpacaNewsStreamingClient
{
    internal AlpacaNewsStreamingClient(
        AlpacaNewsStreamingClientConfiguration configuration)
        : base(configuration.EnsureNotNull())
    {
    }

    public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription() => 
        GetSubscription<INewsArticle, JsonNewsArticle>(NewsChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(
        String symbol) => 
        GetSubscription<INewsArticle, JsonNewsArticle>(NewsChannel, symbol.EnsureNotNull());
}
