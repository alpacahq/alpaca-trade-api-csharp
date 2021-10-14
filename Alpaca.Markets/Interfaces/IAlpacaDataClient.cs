using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
    /// </summary>
    [CLSCompliant(false)]
    public interface IAlpacaDataClient : IDisposable
    {
        /// <summary>
        /// Gets historical bars list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical bars for specified asset (with pagination data).</returns>
        [UsedImplicitly]
        Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical bars dictionary for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary of historical bars for specified assets (with pagination data).</returns>
        [UsedImplicitly]
        Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical quotes list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical quotes for specified asset (with pagination data).</returns>
        [UsedImplicitly]
        Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical quotes dictionary for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary of historical quotes for specified assets (with pagination data).</returns>
        [UsedImplicitly]
        Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical trades list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical trades request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trades for specified asset (with pagination data).</returns>
        [UsedImplicitly]
        Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical trades dictionary for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical trades request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary of historical trades for specified assets (with pagination data).</returns>
        [UsedImplicitly]
        Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets last trade for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        [UsedImplicitly]
        Task<ITrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current quote for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        [UsedImplicitly]
        Task<IQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current snapshot (latest trade/quote and minute/days bars) for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current snapshot information.</returns>
        [UsedImplicitly]
        Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">List of asset names for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary with the current snapshot information.</returns>
        [UsedImplicitly]
        Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets dictionary with exchange code to the exchange name mappings.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [UsedImplicitly]
        Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets dictionary with trades conditions code to the condition description mappings.
        /// </summary>
        /// <param name="tape">SIP tape identifier for retrieving trade conditions.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [UsedImplicitly]
        Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets dictionary with quotes conditions code to the condition description mappings.
        /// </summary>
        /// <param name="tape">SIP tape identifier for retrieving quote conditions.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [UsedImplicitly]
        Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default);
    }
}
