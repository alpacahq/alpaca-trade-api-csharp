using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    [CLSCompliant(false)]
    public interface IAlpacaNewsStreamingClient : IStreamingDataClient
    {
        /// <summary>
        /// Gets the news articles' updates subscription for all stock and crypto assets.
        /// </summary>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<INewsArticle> GetNewsSubscription();

        /// <summary>
        /// Gets the news articles' updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        [UsedImplicitly]
        IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(
            String symbol);
    }
}
