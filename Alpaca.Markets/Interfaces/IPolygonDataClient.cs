using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon Data API via HTTP/REST.
    /// </summary>
    public interface IPolygonDataClient : IDisposable
    {
        /// <summary>
        /// Gets list of available exchanges from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of exchange information objects.</returns>
        Task<IReadOnlyList<IExchange>> ListExchangesAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets mapping dictionary for symbol types from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to symbol type abbreviation and values
        /// equal to full symbol type names descriptions for each supported symbol type.
        /// </returns>
        Task<IReadOnlyDictionary<String, String>> GetSymbolTypeMapAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="request">Historical trades request parameter.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        Task<IHistoricalItems<IHistoricalTrade>> ListHistoricalTradesAsync(
            HistoricalRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameter.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        Task<IHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesAsync(
            HistoricalRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of historical minute bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="request">Day aggregates request parameter.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of day bars for specified asset.</returns>
        Task<IHistoricalItems<IAgg>> ListAggregatesAsync(
            AggregatesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets last trade for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current quote for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets mapping dictionary for specific tick type from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to condition integer values and values
        /// equal to full tick condition descriptions for each supported tick type.
        /// </returns>
        Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets mapping dictionary for specific tick type from Polygon REST API endpoint.
        /// </summary>
        /// <param name="tickType">Tick type for conditions map.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to condition integer values and values
        /// equal to full tick condition descriptions for each supported tick type.
        /// </returns>
        Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            TickType tickType,
            CancellationToken cancellationToken = default);
    }
}
