using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataStreamingClient"/> interface.
    /// </summary>
    public static partial class AlpacaDataStreamingClientExtensions
    {
        /// <summary>
        /// Gets the trading statuses updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IStatus> GetStatusSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getStatusSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trading statuses subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IStatus> GetStatusSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getStatusSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));
        
        /// <summary>
        /// Gets the LULD (limit up / limit down) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getLimitUpLimitDownSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the LULD (limit up / limit down) subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getLimitUpLimitDownSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trading status updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetStatusSubscription(symbol),
                client);

        /// <summary>
        /// Gets the trading status updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetStatusSubscription(symbols),
                client);

        /// <summary>
        /// Gets the trading status updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetStatusSubscription(symbols),
                client);

        /// <summary>
        /// Gets the LULD (limit up / limit down) subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetLimitUpLimitDownSubscription(symbol),
                client);

        /// <summary>
        /// Gets the LULD (limit up / limit down) subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetLimitUpLimitDownSubscription(symbols),
                client);

        /// <summary>
        /// Gets the LULD (limit up / limit down) subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetLimitUpLimitDownSubscription(symbols),
                client);

        private static IAlpacaDataSubscription<IStatus> getStatusSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetStatusSubscription, symbols);

        private static IAlpacaDataSubscription<ILimitUpLimitDown> getLimitUpLimitDownSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetLimitUpLimitDownSubscription, symbols);

        private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
            Func<String, IAlpacaDataSubscription<TItem>> selector,
            IEnumerable<String> symbols) =>
            new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
    }
}
