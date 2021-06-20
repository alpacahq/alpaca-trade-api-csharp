using System;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IAlpacaStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    public static class AlpacaStreamingClientExtensions
    {
        private sealed class ClientWithReconnection :
            ClientWithReconnectBase<IAlpacaStreamingClient>,
            IAlpacaStreamingClient
        {
            public ClientWithReconnection(
                IAlpacaStreamingClient client,
                ReconnectionParameters reconnectionParameters)
                : base (client, reconnectionParameters)
            {
            }

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
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaStreamingClient WithReconnect(
            this IAlpacaStreamingClient client) =>
            WithReconnect(client, ReconnectionParameters.Default);

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaStreamingClient WithReconnect(
            this IAlpacaStreamingClient client,
            ReconnectionParameters parameters) =>
            new ClientWithReconnection(client, parameters);
    }
}
