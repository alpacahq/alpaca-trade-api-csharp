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
        /// Gets the trade updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getMinuteBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the daily aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            getDailyBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

        /// <summary>
        /// Gets the daily aggregate (bar) updates subscription for the all assets from the <paramref name="symbols"/> list.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getDailyBarSubscription(
                client.EnsureNotNull(nameof(client)),
                symbols.EnsureNotNull(nameof(symbols)));

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
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset. This subscription is
        /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
        /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
        /// <param name="symbol">Alpaca asset name for trade updates subscribing.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{ITrade}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeMinuteBarAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetMinuteBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for the <paramref name="symbol"/> asset. This subscription is
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
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IAlpacaDataStreamingClient client,
            String symbol) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbol),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for all assets from the <paramref name="symbols"/> list.
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
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IAlpacaDataStreamingClient client,
            params String[] symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbols),
                client);

        /// <summary>
        /// Gets the daily bar updates subscription for all assets from the <paramref name="symbols"/> list.
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
        public static ValueTask<IDisposableAlpacaDataSubscription<IBar>> SubscribeDailyBarAsync(
            this IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            DisposableAlpacaDataSubscription<IBar>.CreateAsync(
                client.EnsureNotNull(nameof(client)).GetDailyBarSubscription(symbols),
                client);

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

        private static IAlpacaDataSubscription<IBar> getDailyBarSubscription(
            IAlpacaDataStreamingClient client,
            IEnumerable<String> symbols) =>
            getSubscription(client.GetDailyBarSubscription, symbols);

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
