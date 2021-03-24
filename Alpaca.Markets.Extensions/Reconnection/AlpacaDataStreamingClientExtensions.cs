using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IAlpacaDataStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class AlpacaDataStreamingClientExtensions
    {
        private sealed class ClientWithReconnection :
            DataClientWithReconnectBase<IAlpacaDataStreamingClient>,
            IAlpacaDataStreamingClient
        {
            public ClientWithReconnection(
                IAlpacaDataStreamingClient client,
                ReconnectionParameters reconnectionParameters)
                : base (client, reconnectionParameters)
            {
            }

            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() =>
                Client.GetMinuteAggSubscription();

            public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(String symbol) =>
                Client.GetTradeSubscription(symbol);

            public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(String symbol) =>
                Client.GetQuoteSubscription(symbol);

            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(String symbol) =>
                Client.GetMinuteAggSubscription(symbol);
        }

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        [CLSCompliant(false)]
        public static IAlpacaDataStreamingClient WithReconnect(
            this IAlpacaDataStreamingClient client) =>
            WithReconnect(client, ReconnectionParameters.Default);

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
        /// with automatic reconnection support with the default reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters.</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        [CLSCompliant(false)]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static IAlpacaDataStreamingClient WithReconnect(
            this IAlpacaDataStreamingClient client,
            ReconnectionParameters parameters) =>
            new ClientWithReconnection(client, parameters);
    }
}
