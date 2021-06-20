using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IAlpacaDataStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
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
                IAlpacaDataSubscription subscription,
                CancellationToken cancellationToken = default)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryAdd(stream, subscription);
                }

                return Client.SubscribeAsync(subscription, cancellationToken);
            }

            public ValueTask SubscribeAsync(
                IEnumerable<IAlpacaDataSubscription> subscriptions,
                CancellationToken cancellationToken = default)
            {
                var dataSubscriptions = new List<IAlpacaDataSubscription>(subscriptions);

                foreach (var subscription in dataSubscriptions)
                {
                    foreach (var stream in subscription.Streams)
                    {
                        _subscriptions.TryAdd(stream, subscription);
                    }
                }

                return Client.SubscribeAsync(dataSubscriptions, cancellationToken);
            }

            public ValueTask UnsubscribeAsync(
                IAlpacaDataSubscription subscription,
                CancellationToken cancellationToken = default)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryRemove(stream, out _);
                }

                return Client.UnsubscribeAsync(subscription, cancellationToken);
            }

            public ValueTask UnsubscribeAsync(
                IEnumerable<IAlpacaDataSubscription> subscriptions,
                CancellationToken cancellationToken = default)
            {
                var dataSubscriptions = new List<IAlpacaDataSubscription>(subscriptions);

                foreach (var stream in dataSubscriptions
                    .SelectMany(subscription => subscription.Streams))
                {
                    _subscriptions.TryRemove(stream, out _);
                }

                return Client.UnsubscribeAsync(dataSubscriptions, cancellationToken);
            }

            protected override ValueTask OnReconnection(
                CancellationToken cancellationToken) =>
                Client.SubscribeAsync(_subscriptions.Values, cancellationToken);
        }

        /// <summary>
        /// Wraps instance of <see cref="IAlpacaDataStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        [UsedImplicitly]
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
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataStreamingClient WithReconnect(
            this IAlpacaDataStreamingClient client,
            ReconnectionParameters parameters) =>
            new ClientWithReconnection(client, parameters);
    }
}
