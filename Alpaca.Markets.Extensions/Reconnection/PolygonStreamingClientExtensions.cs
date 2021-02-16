using System;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IPolygonStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    public static partial class PolygonStreamingClientExtensions
    {
        private sealed class ClientWithReconnection :
            DataClientWithReconnectBase<IPolygonStreamingClient>,
            IPolygonStreamingClient
        {
            public ClientWithReconnection(
                IPolygonStreamingClient client, 
                ReconnectionParameters reconnectionParameters)
                : base(client, reconnectionParameters)
            {
            }
            
            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() =>
                Client.GetMinuteAggSubscription();

            public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription() => 
                Client.GetSecondAggSubscription();

            public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(String symbol) =>
                Client.GetTradeSubscription(symbol);

            public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(String symbol) =>
                Client.GetQuoteSubscription(symbol);

            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(String symbol) =>
                Client.GetMinuteAggSubscription(symbol);

            public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription(String symbol) => 
                Client.GetSecondAggSubscription(symbol);
        }

        /// <summary>
        /// Wraps instance of <see cref="IPolygonStreamingClient"/> into the helper class
        /// with automatic reconnection support with the default reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IPolygonStreamingClient WithReconnect(
            this IPolygonStreamingClient client) =>
            WithReconnect(client, ReconnectionParameters.Default);

        /// <summary>
        /// Wraps instance of <see cref="IPolygonStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IPolygonStreamingClient WithReconnect(
            this IPolygonStreamingClient client,
            ReconnectionParameters parameters) =>
            new ClientWithReconnection(client, parameters);
    }
}
