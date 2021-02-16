using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IPolygonStreamingClient"/> interface.
    /// </summary>
    public static partial class PolygonStreamingClientExtensions
    {

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            this IPolygonStreamingClient client,
            params String[] symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            this IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            this IPolygonStreamingClient client,
            params String[] symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            this IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            this IPolygonStreamingClient client,
            params String[] symbols) =>
            getMinuteAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            this IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getMinuteAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the second aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription(
            this IPolygonStreamingClient client,
            params String[] symbols) =>
            getSecondAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the second aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IPolygonStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        public static IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription(
            this IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getSecondAggSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        private static IAlpacaDataSubscription<IStreamTrade> getTradeSubscription(
            IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetTradeSubscription, symbols);

        private static IAlpacaDataSubscription<IStreamQuote> getQuoteSubscription(
            IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetQuoteSubscription, symbols);

        private static IAlpacaDataSubscription<IStreamAgg> getMinuteAggSubscription(
            IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetMinuteAggSubscription, symbols);

        private static IAlpacaDataSubscription<IStreamAgg> getSecondAggSubscription(
            IPolygonStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetSecondAggSubscription, symbols);

        private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
            Func<String, IAlpacaDataSubscription<TItem>> selector,
            IEnumerable<String> symbols) 
            where TItem : IStreamBase =>
            new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
    }
}
