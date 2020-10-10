using System;
using System.Linq;
using System.Collections.Generic;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IAlpacaDataStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    public static class AlpacaDataStreamingClientExtensions
    {
        private sealed class ClientWithReconnection :
            ClientWithReconnectBase<IAlpacaDataStreamingClient, IAlpacaDataSubscription>,
            IAlpacaDataStreamingClient
        {
            public ClientWithReconnection(
                IAlpacaDataStreamingClient client,
                ReconnectionParameters reconnectionParameters)
                : base (client, reconnectionParameters)
            {
            }

            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() =>
                _client.GetMinuteAggSubscription();

            public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(String symbol) =>
                _client.GetTradeSubscription(symbol);

            public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(String symbol) =>
                _client.GetQuoteSubscription(symbol);

            public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(String symbol) =>
                _client.GetMinuteAggSubscription(symbol);

            public void Subscribe(
                IAlpacaDataSubscription subscription)
            {
                _subscriptions.TryAdd(subscription.Stream, subscription);
                _client.Subscribe(subscription);
            }

            public void Subscribe(
                IEnumerable<IAlpacaDataSubscription> subscriptions) => 
                Subscribe(subscriptions.ToArray());

            public void Subscribe(
                params IAlpacaDataSubscription[] subscriptions)
            {
                foreach (var subscription in subscriptions)
                {
                    _subscriptions.TryAdd(subscription.Stream, subscription);
                }
                _client.Subscribe(subscriptions);
            }

            public void Unsubscribe(
                IAlpacaDataSubscription subscription)
            {
                _subscriptions.TryRemove(subscription.Stream, out _);
                _client.Unsubscribe(subscription);
            }

            public void Unsubscribe(
                IEnumerable<IAlpacaDataSubscription> subscriptions) =>
                Unsubscribe(subscriptions.ToArray());

            public void Unsubscribe(
                params IAlpacaDataSubscription[] subscriptions)
            {
                foreach (var subscription in subscriptions)
                {
                    _subscriptions.TryRemove(subscription.Stream, out _);
                }
                _client.Unsubscribe(subscriptions);
            }

            protected override void Resubscribe(
                String symbol, 
                IAlpacaDataSubscription subscription)
            {
                _client.Subscribe(subscription);
            }
        }

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IAlpacaDataStreamingClient WithReconnect(
            this IAlpacaDataStreamingClient client,
            ReconnectionParameters? parameters = null) =>
            new ClientWithReconnection(client, parameters ?? ReconnectionParameters.Default);
    }
}
