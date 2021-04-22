using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [CLSCompliant(false)]
    public interface IAlpacaDataStreamingClient : IStreamingDataClient
    {
        /// <summary>
        /// Gets the trade updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        IAlpacaDataSubscription<ITrade> GetTradeSubscription(
            String symbol);

        /// <summary>
        /// Gets the quote updates subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
            String symbol);

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the all assets.
        /// </summary>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        IAlpacaDataSubscription<IBar> GetMinuteBarSubscription();

        /// <summary>
        /// Gets the minute aggregate (bar) subscription for the <paramref name="symbol"/> asset.
        /// </summary>
        /// <param name="symbol">Alpaca asset name.</param>
        /// <returns>
        /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
        /// </returns>
        IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
            String symbol);
    }
}
