namespace Alpaca.Markets.Extensions;

/// <summary>
/// Helper extension method for creating special version of the <see cref="IAlpacaNewsStreamingClient"/>
/// implementation with automatic reconnection (with configurable delay and number of attempts) support.
/// </summary>
public static partial class AlpacaNewsStreamingClientExtensions
{
    private sealed class ClientWithReconnection(
        IAlpacaNewsStreamingClient client,
        ReconnectionParameters reconnectionParameters) :
        ClientWithSubscriptionReconnectBase<IAlpacaNewsStreamingClient>(
            client, reconnectionParameters),
        IAlpacaNewsStreamingClient
    {
        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription() =>
            Client.GetNewsSubscription();

        [ExcludeFromCodeCoverage]
        public IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(String symbol) =>
            Client.GetNewsSubscription(symbol);
    }

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaNewsStreamingClient"/> into the helper class
    /// with automatic reconnection support and provide optional reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaNewsStreamingClient WithReconnect(
        this IAlpacaNewsStreamingClient client) =>
        WithReconnect(client.EnsureNotNull(), ReconnectionParameters.Default);

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
    /// with automatic reconnection support with the default reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <param name="parameters">Reconnection parameters.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="parameters"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaNewsStreamingClient WithReconnect(
        this IAlpacaNewsStreamingClient client,
        ReconnectionParameters parameters) =>
        new ClientWithReconnection(client.EnsureNotNull(), parameters.EnsureNotNull());
}
