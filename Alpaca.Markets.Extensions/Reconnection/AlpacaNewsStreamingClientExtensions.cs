namespace Alpaca.Markets.Extensions;

/// <summary>
/// Helper extension method for creating special version of the <see cref="IAlpacaNewsStreamingClient"/>
/// implementation with automatic reconnection (with configurable delay and number of attempts) support.
/// </summary>
public static partial class AlpacaNewsStreamingClientExtensions
{
    private sealed class ClientWithReconnection :
        ClientWithSubscriptionReconnectBase<IAlpacaNewsStreamingClient>,
        IAlpacaNewsStreamingClient
    {
        public ClientWithReconnection(
            IAlpacaNewsStreamingClient client,
            ReconnectionParameters reconnectionParameters)
            : base (client, reconnectionParameters)
        {
        }

        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription() =>
            Client.GetNewsSubscription();

        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(String symbol) =>
            Client.GetNewsSubscription(symbol);
    }

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaNewsStreamingClient"/> into the helper class
    /// with automatic reconnection support and provide optional reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaNewsStreamingClient WithReconnect(
        this IAlpacaNewsStreamingClient client) =>
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
    public static IAlpacaNewsStreamingClient WithReconnect(
        this IAlpacaNewsStreamingClient client,
        ReconnectionParameters parameters) =>
        new ClientWithReconnection(client, parameters);
}
