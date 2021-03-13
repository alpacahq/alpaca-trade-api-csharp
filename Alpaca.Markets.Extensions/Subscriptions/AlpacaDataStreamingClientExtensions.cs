using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataStreamingClient"/> interface.
    /// </summary>
    public static partial class AlpacaDataStreamingClientExtensions
    {
        private sealed class MultiSubscription<TItem>
            : IAlpacaDataSubscription<TItem> 
        {
            private readonly IReadOnlyList<IAlpacaDataSubscription<TItem>> _subscriptions;

            public MultiSubscription(
                IEnumerable<IAlpacaDataSubscription<TItem>> subscriptions) =>
                _subscriptions = subscriptions.ToList();

            public IEnumerable<String> Streams => _subscriptions.SelectMany(_ => _.Streams);

            public Boolean Subscribed => _subscriptions.All(_ => _.Subscribed);

            public event Action<TItem>? Received
            {
                add
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Received += value;
                    }
                }
                remove
                {
                    foreach (var subscription in _subscriptions)
                    {
                        subscription.Received -= value;
                    }
                }
            }
        }

        private sealed class DisposableAlpacaDataSubscription<TItem> :
            IDisposableAlpacaDataSubscription<TItem>
        {
            private readonly IAlpacaDataSubscription<TItem> _subscription;

            private readonly IAlpacaDataStreamingClient _client;

            public DisposableAlpacaDataSubscription(
                IAlpacaDataSubscription<TItem> subscription,
                IAlpacaDataStreamingClient client)
            {
                _subscription = subscription;
                _client = client;

                _client.Subscribe(_subscription);
            }

            public IEnumerable<String> Streams => _subscription.Streams;

            public Boolean Subscribed => _subscription.Subscribed;

            public event Action<TItem> Received
            {
                add => _subscription.Received += value;
                remove => _subscription.Received -= value;
            }

            public ValueTask DisposeAsync() => 
                new (Task.Run(Dispose));

            public void Dispose() => 
                _client.Unsubscribe(_subscription);
        }

        private const Int32 MaxAllowedTradeOrQuoteSubscriptionsCount = 30;

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamQuote}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamQuote}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getMinuteAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getMinuteAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamTrade> SubscribeTrade(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            new DisposableAlpacaDataSubscription<IStreamTrade>(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbol),
                client);

        /// <summary>
        /// Gets the trade updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamTrade> SubscribeTrade(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            new DisposableAlpacaDataSubscription<IStreamTrade>(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbols),
                client);

        /// <summary>
        /// Gets the trade updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamTrade> SubscribeTrade(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            new DisposableAlpacaDataSubscription<IStreamTrade>(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbols),
                client);

        /// <summary>
        /// Gets the quote updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamQuote}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamQuote> SubscribeQuote(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            new DisposableAlpacaDataSubscription<IStreamQuote>(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbol),
                client);

        /// <summary>
        /// Gets the quote updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamQuote> SubscribeQuote(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            new DisposableAlpacaDataSubscription<IStreamQuote>(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbols),
                client);

        /// <summary>
        /// Gets the quote updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamQuote> SubscribeQuote(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            new DisposableAlpacaDataSubscription<IStreamQuote>(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbols),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamAgg> SubscribeMinuteBar(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            new DisposableAlpacaDataSubscription<IStreamAgg>(
                client.EnsureNotNull(nameof(client)).GetMinuteAggSubscription(symbol),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamAgg> SubscribeMinuteBar(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            new DisposableAlpacaDataSubscription<IStreamAgg>(
                client.EnsureNotNull(nameof(client)).GetMinuteAggSubscription(symbols),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        public static IDisposableAlpacaDataSubscription<IStreamAgg> SubscribeMinuteBar(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            new DisposableAlpacaDataSubscription<IStreamAgg>(
                client.EnsureNotNull(nameof(client)).GetMinuteAggSubscription(symbols),
                client);

        private static IAlpacaDataSubscription<IStreamTrade> getTradeSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetTradeSubscription, symbols.takeNotMoreThan(MaxAllowedTradeOrQuoteSubscriptionsCount));

        private static IAlpacaDataSubscription<IStreamQuote> getQuoteSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetQuoteSubscription, symbols.takeNotMoreThan(MaxAllowedTradeOrQuoteSubscriptionsCount));

        private static IAlpacaDataSubscription<IStreamAgg> getMinuteAggSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetMinuteAggSubscription, symbols);

        private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
            Func<String, IAlpacaDataSubscription<TItem>> selector,
            IEnumerable<String> symbols) =>
            new MultiSubscription<TItem>(symbols.Select(selector));

        private static IEnumerable<T> takeNotMoreThan<T>(
            this IEnumerable<T> source,
            Int32 count)
        {
            foreach (var item in source)
            {
                if (--count < 0)
                {
                    throw new InvalidOperationException(
                        "Too many symbols in single subscription request.");
                }

                yield return item;
            }
        }
    }
}
