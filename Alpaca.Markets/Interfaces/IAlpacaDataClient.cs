using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
    /// </summary>
    public interface IAlpacaDataClient : IDisposable
    {
        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical daily bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        [Obsolete("The underlying endpoint will be deprecated soon - use the `ListHistoricalBarsAsync` method instead.", false)]
        Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets last trade for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        [Obsolete("The underlying endpoint will be deprecated soon - use the `GetLatestTradeAsync` method instead.", false)]
        Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current quote for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        [Obsolete("The underlying endpoint will be deprecated soon - use the `GetLatestQuoteAsync` method instead.", false)]
        Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical bars list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical bars for specified asset (with pagination data).</returns>
        Task<IPage<IAgg>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical quotes list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical quotes for specified asset (with pagination data).</returns>
        Task<IPage<IHistoricalQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical trades list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical trades request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trades for specified asset (with pagination data).</returns>
        Task<IPage<IHistoricalTrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets last trade for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        Task<IStreamTrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current quote for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        Task<IStreamQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current snapshot (latest trade/quote and minute/days bars) for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current snapshot information.</returns>
        Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">List of asset names for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary with the current snapshot information.</returns>
        Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default);
    }
}
