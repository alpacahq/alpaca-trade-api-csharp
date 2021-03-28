using System;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IAlpacaStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    public static class AlpacaStreamingClientExtensions
    {
        private sealed class ClientWithReconnection :
            ClientWithReconnectBase<IAlpacaStreamingClient, Int32>,
            IAlpacaStreamingClient
        {
            public ClientWithReconnection(
                IAlpacaStreamingClient client,
                ReconnectionParameters reconnectionParameters)
                : base (client, reconnectionParameters)
            {
            }

#pragma warning disable CS0067 // Event never used
            [Obsolete("This event never raised and will be removed in the next major SDK release", true)]
            public event Action<IAccountUpdate>? OnAccountUpdate;
#pragma warning restore CS0067 // Event never used

            public event Action<ITradeUpdate>? OnTradeUpdate
            {
                add => Client.OnTradeUpdate += value;
                remove => Client.OnTradeUpdate += value;
            }

            protected override void Resubscribe(String symbol, Int32 subscription) { }
        }

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IAlpacaStreamingClient WithReconnect(
            this IAlpacaStreamingClient client,
            ReconnectionParameters? parameters = null) =>
            new ClientWithReconnection(client, parameters ?? ReconnectionParameters.Default);
    }
}
