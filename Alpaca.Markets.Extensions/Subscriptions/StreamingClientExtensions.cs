using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IStreamingDataClient"/> interface.
    /// </summary>
    public static partial class StreamingClientExtensions
    {
        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ITrade> GetTradeSubscription(
            this IStreamingDataClient client,
            params String[] symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<ITrade> GetTradeSubscription(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getTradeSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            this IStreamingDataClient client,
            params String[] symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the quote updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getQuoteSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            this IStreamingDataClient client,
            params String[] symbols) =>
            getMinuteBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the minute aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getMinuteBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the daily aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            this IStreamingDataClient client,
            params String[] symbols) =>
            getDailyBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the Daily aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getDailyBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IStreamingDataClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbol),
                client);

        /// <summary>
        /// Gets the trade updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IStreamingDataClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbols),
                client);

        /// <summary>
        /// Gets the trade updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeTradeAsync(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetTradeSubscription(symbols),
                client);

        /// <summary>
        /// Gets the quote updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IQuote}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IStreamingDataClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbol),
                client);

        /// <summary>
        /// Gets the quote updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IStreamingDataClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbols),
                client);

        /// <summary>
        /// Gets the quote updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for quote updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IQuote>> SubscribeQuoteAsync(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IQuote>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetQuoteSubscription(symbols),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IBar}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IStreamingDataClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbol),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IStreamingDataClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the minute bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IStreamingDataClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbol),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IStreamingDataClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for all assets from the <paramref name="symbols"/> list.
        /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
        /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbols),
                client);

        private static IAlpacaDataSubscription<ITrade> getTradeSubscription(
            IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetTradeSubscription, symbols);

        private static IAlpacaDataSubscription<IQuote> getQuoteSubscription(
            IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetQuoteSubscription, symbols);

        private static IAlpacaDataSubscription<IBar> getMinuteBarSubscription(
            IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetMinuteBarSubscription, symbols);

        private static IAlpacaDataSubscription<IBar> getDailyBarSubscription(
            IStreamingDataClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetDailyBarSubscription, symbols);

        private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
            Func<String, IAlpacaDataSubscription<TItem>> selector,
            IEnumerable<String> symbols) =>
            new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
    }
}
