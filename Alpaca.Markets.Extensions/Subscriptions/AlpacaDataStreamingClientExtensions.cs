using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataStreamingClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static partial class AlpacaDataStreamingClientExtensions
    {
        private sealed class AlpacaDataSubscriptionContainer<TItem>
            : IAlpacaDataSubscription<TItem>
        {
            private readonly IReadOnlyList<IAlpacaDataSubscription<TItem>> _subscriptions;

            public AlpacaDataSubscriptionContainer(
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

            private IAlpacaDataStreamingClient? _client;

            private DisposableAlpacaDataSubscription(
                IAlpacaDataSubscription<TItem> subscription,
                IAlpacaDataStreamingClient client)
            {
                _subscription = subscription;
                _client = client;
            }

            public static async ValueTask<IDisposableAlpacaDataSubscription<TItem>> CreateAsync(
                IAlpacaDataSubscription<TItem> subscription,
                IAlpacaDataStreamingClient client)
            {
                await client.SubscribeAsync(subscription).ConfigureAwait(false);
                return new DisposableAlpacaDataSubscription<TItem>(subscription, client);
            }

            public IEnumerable<String> Streams => _subscription.Streams;

            public Boolean Subscribed => _subscription.Subscribed;

            public event Action<TItem> Received
            {
                add => _subscription.Received += value;
                remove => _subscription.Received -= value;
            }

            public async void Dispose() => 
                await DisposeAsync().ConfigureAwait(false);

            public async ValueTask DisposeAsync()
            {
                if (_client is null)
                {
                    return;
                }

                await _client.UnsubscribeAsync(_subscription)
                    .ConfigureAwait(false);
                _client = null;
            }
        }

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ITrade> GetTradeSubscription(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ITrade> GetTradeSubscription(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getMinuteBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getMinuteBarSubscription(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbol),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbols),
                client);

        private static IAlpacaDataSubscription<ITrade> getTradeSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetTradeSubscription, symbols);

        private static IAlpacaDataSubscription<IQuote> getQuoteSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetQuoteSubscription, symbols);

        private static IAlpacaDataSubscription<IBar> getMinuteBarSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetMinuteBarSubscription, symbols);

        private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
            Func<String, IAlpacaDataSubscription<TItem>> selector,
            IEnumerable<String> symbols) =>
            new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
    }
}
