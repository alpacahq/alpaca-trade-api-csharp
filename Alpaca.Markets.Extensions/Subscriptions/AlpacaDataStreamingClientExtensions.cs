using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataStreamingClient"/> interface.
    /// </summary>
    public static partial class AlpacaDataStreamingClientExtensions
    {
        private sealed class MultiSubscription<TItem>
            : IAlpacaDataSubscription<TItem> 
            where TItem : IStreamBase
        {
            private readonly IReadOnlyList<IAlpacaDataSubscription<TItem>> _subscriptions;

            public MultiSubscription(
                IEnumerable<IAlpacaDataSubscription<TItem>> subscriptions) =>
                _subscriptions = subscriptions.ToList();

            public String Stream => Streams.FirstOrDefault() ?? String.Empty;

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

        private const Int32 MaxAllowedTradeOrQuoteSubscriptionsCount = 30;

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
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
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getMinuteAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

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
            IEnumerable<String> symbols) 
            where TItem : IStreamBase =>
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
