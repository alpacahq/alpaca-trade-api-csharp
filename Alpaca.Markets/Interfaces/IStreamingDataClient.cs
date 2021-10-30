using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs with data subscriptions.
    /// </summary>
    [CLSCompliant(false)]
    public interface IStreamingDataClient : IStreamingClient
    {
        /// <summary>
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<ITrade> GetTradeSubscription(
            String symbol);

        /// <summary>
        /// Gets the quote updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol);

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the all assets.
        /// </summary>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IBar> GetMinuteBarSubscription();

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            String symbol);

        /// <summary>
        /// Gets the daily aggregate (bar) subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
            String symbol);

        /// <summary>
        /// Subscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask SubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask SubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask UnsubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask UnsubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);
    }
}
