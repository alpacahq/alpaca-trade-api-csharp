using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    public interface IAlpacaDataStreamingClient : IStreamingClientBase
    {
        /// <summary>
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol);

        /// <summary>
        /// Gets the quote updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol);

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the all assets.
        /// </summary>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription();

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol);

        /// <summary>
        /// Gets the daily aggregate (bar) subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<IStreamAgg> GetDailyAggSubscription(
            String symbol);

        /// <summary>
        /// Subscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        void Subscribe(
            IAlpacaDataSubscription subscription);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        [UsedImplicitly]
        void Subscribe(
            params IAlpacaDataSubscription[] subscriptions);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        [UsedImplicitly]
        void Subscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions);

        /// <summary>
        /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        [UsedImplicitly]
        void Unsubscribe(
            IAlpacaDataSubscription subscription);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        [UsedImplicitly]
        void Unsubscribe(
            params IAlpacaDataSubscription[] subscriptions);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        [UsedImplicitly]
        void Unsubscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions);
    }
}
