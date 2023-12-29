namespace Alpaca.Markets.Extensions;

/// <summary>
/// Helper extension method for creating special version of the <see cref="IAlpacaStreamingClient"/>
/// implementation with automatic reconnection (with configurable delay and number of attempts) support.
/// </summary>
public static class AlpacaStreamingClientExtensions
{
    private sealed class ClientWithReconnection(
        IAlpacaStreamingClient client,
        ReconnectionParameters reconnectionParameters) :
        ClientWithReconnectBase<IAlpacaStreamingClient>(
            client, reconnectionParameters),
        IAlpacaStreamingClient
    {
        /// <inheritdoc cref="IAlpacaStreamingClient.OnTradeUpdate"/>
        public event Action<ITradeUpdate>? OnTradeUpdate
        {
            add => Client.OnTradeUpdate += value;
            remove => Client.OnTradeUpdate += value;
        }
    }

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaStreamingClient"/> into the helper class
    /// with automatic reconnection support with the default reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaStreamingClient WithReconnect(
        this IAlpacaStreamingClient client) =>
        WithReconnect(client.EnsureNotNull(), ReconnectionParameters.Default);

    /// <summary>
    /// Wraps instance of <see cref="IAlpacaStreamingClient"/> into the helper class
    /// with automatic reconnection support and provide optional reconnection parameters.
    /// </summary>
    /// <param name="client">Original streaming client for wrapping.</param>
    /// <param name="parameters">Reconnection parameters (or default if missing).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="parameters"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaStreamingClient WithReconnect(
        this IAlpacaStreamingClient client,
        ReconnectionParameters parameters) =>
        new ClientWithReconnection(client.EnsureNotNull(), parameters.EnsureNotNull());
}
