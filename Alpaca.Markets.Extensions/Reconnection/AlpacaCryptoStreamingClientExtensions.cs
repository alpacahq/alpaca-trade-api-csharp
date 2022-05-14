namespace Alpaca.Markets.Extensions;

/// <summary>
/// Helper extension method for creating special version of the <see cref="IAlpacaCryptoStreamingClient"/>
/// implementation with automatic reconnection (with configurable delay and number of attempts) support.
/// </summary>
public static partial class AlpacaCryptoStreamingClientExtensions
{
    private sealed class ClientWithReconnection :
        ClientWithSubscriptionReconnectBase<IAlpacaCryptoStreamingClient>,
        IAlpacaCryptoStreamingClient
    {
        public ClientWithReconnection(
            IAlpacaCryptoStreamingClient client,
            ReconnectionParameters reconnectionParameters)
            : base(client, reconnectionParameters)
        {
        }

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<ITrade> GetTradeSubscription(String symbol) =>
            Client.GetTradeSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(String symbol) =>
            Client.GetQuoteSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(String symbol) =>
            Client.GetMinuteBarSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
            Client.GetMinuteBarSubscription();

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(String symbol) =>
            Client.GetDailyBarSubscription(symbol);

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IBar> GetUpdatedBarSubscription(String symbol) =>
            Client.GetUpdatedBarSubscription(symbol);


        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(String symbol) =>
            Client.GetOrderBookSubscription(symbol);
    }

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaCryptoStreamingClient"/> into the helper class
    /// with automatic reconnection support and provide optional reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoStreamingClient WithReconnect(
        this IAlpacaCryptoStreamingClient client) =>
        WithReconnect(client, ReconnectionParameters.Default);

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
    /// with automatic reconnection support with the default reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <param name="parameters">Reconnection parameters.</param>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoStreamingClient WithReconnect(
        this IAlpacaCryptoStreamingClient client,
        ReconnectionParameters parameters) =>
        new ClientWithReconnection(client, parameters);
}
