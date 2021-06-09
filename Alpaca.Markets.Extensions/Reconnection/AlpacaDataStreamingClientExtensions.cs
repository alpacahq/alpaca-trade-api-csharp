using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            ClientWithReconnectBase<IAlpacaDataStreamingClient>,
            IAlpacaDataStreamingClient
        {
            private readonly ConcurrentDictionary<String, IAlpacaDataSubscription> _subscriptions =
                new(StringComparer.Ordinal);

            public ClientWithReconnection(
                IAlpacaDataStreamingClient client,
                ReconnectionParameters reconnectionParameters)
                : base (client, reconnectionParameters)
            {
            }

            public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
                Client.GetMinuteBarSubscription();

            public IAlpacaDataSubscription<ITrade> GetTradeSubscription(String symbol) =>
                Client.GetTradeSubscription(symbol);

            public IAlpacaDataSubscription<IQuote> GetQuoteSubscription(String symbol) =>
                Client.GetQuoteSubscription(symbol);

            public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(String symbol) =>
                Client.GetMinuteBarSubscription(symbol);

            public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(String symbol) =>
                Client.GetDailyBarSubscription(symbol);

            public ValueTask SubscribeAsync(
                IAlpacaDataSubscription subscription)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryAdd(stream, subscription);
                }

                return Client.SubscribeAsync(subscription);
            }

            public ValueTask SubscribeAsync(
                IEnumerable<IAlpacaDataSubscription> subscriptions) =>
                SubscribeAsync(subscriptions.ToArray());

            public ValueTask SubscribeAsync(
                params IAlpacaDataSubscription[] subscriptions)
            {
                foreach (var subscription in subscriptions)
                {
                    foreach (var stream in subscription.Streams)
                    {
                        _subscriptions.TryAdd(stream, subscription);
                    }
                }

                return Client.SubscribeAsync(subscriptions);
            }

            public ValueTask UnsubscribeAsync(
                IAlpacaDataSubscription subscription)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryRemove(stream, out _);
                }

                return Client.UnsubscribeAsync(subscription);
            }

            public ValueTask UnsubscribeAsync(
                IEnumerable<IAlpacaDataSubscription> subscriptions) =>
                UnsubscribeAsync(subscriptions.ToArray());

            public ValueTask UnsubscribeAsync(
                params IAlpacaDataSubscription[] subscriptions)
            {
                foreach (var subscription in subscriptions)
                {
                    foreach (var stream in subscription.Streams)
                    {
                        _subscriptions.TryRemove(stream, out _);
                    }
                }

                return Client.UnsubscribeAsync(subscriptions);
            }

            protected override ValueTask OnReconnection(
                CancellationToken cancellationToken) =>
                Client.SubscribeAsync(_subscriptions.Values);
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
