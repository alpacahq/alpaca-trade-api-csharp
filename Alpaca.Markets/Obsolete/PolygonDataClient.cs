using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class PolygonDataClient
    {
        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="timestamp">Paging - Using the timestamp of the last result will give you the next page of results.</param>
        /// <param name="timestampLimit">Maximum timestamp allowed in the results.</param>
        /// <param name="limit">Limits the size of the response.</param>
        /// <param name="reverse">Reverses the order of the results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        [Obsolete("Use overloaded method that required HistoricalRequest parameter instead of this one.", false)]
        public Task<IHistoricalItems<IHistoricalTrade>> ListHistoricalTradesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default) =>
            ListHistoricalTradesAsync(
                new HistoricalRequest(symbol, date)
                {
                    Limit = limit,
                    Reverse = reverse,
                    Timestamp = timestamp,
                    TimestampLimit = timestampLimit
                }, cancellationToken);

        /// <summary>
        /// Gets list of historical trades for single asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="offset">Paging - offset or first historical trade in days trades list.</param>
        /// <param name="limit">Paging - maximal number of historical trades in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        [Obsolete("This version of ListHistoricalTradesAsync will be deprecated in a future release.", true)]
        public Task<IDayHistoricalItems<IHistoricalTrade>> ListHistoricalTradesV1Async(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"v1/historic/trades/{symbol}/{date.AsDateString()}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return _httpClient.GetSingleObjectAsync
            <IDayHistoricalItems<IHistoricalTrade>, JsonDayHistoricalItems<IHistoricalTrade, JsonHistoricalTradeV1>
            >(
                FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of historical trades for a single asset from Polygon's REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="timestamp">Paging - Using the timestamp of the last result will give you the next page of results.</param>
        /// <param name="timestampLimit">Maximum timestamp allowed in the results.</param>
        /// <param name="limit">Limits the size of the response.</param>
        /// <param name="reverse">Reverses the order of the results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        [Obsolete("Use overloaded method that required HistoricalRequest parameter instead of this one.", false)]
        public Task<IHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default) =>
            ListHistoricalQuotesAsync(
                new HistoricalRequest(symbol, date)
                {
                    Limit = limit,
                    Reverse = reverse,
                    Timestamp = timestamp,
                    TimestampLimit = timestampLimit
                }, cancellationToken);

        /// <summary>
        /// Gets list of historical quotes for single asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="offset">Paging - offset or first historical quote in days quotes list.</param>
        /// <param name="limit">Paging - maximal number of historical quotes in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical quote information.</returns>
        [Obsolete("This version of ListHistoricalQuotesAsync will be deprecated in a future release.", true)]
        public Task<IDayHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesV1Async(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"v1/historic/quotes/{symbol}/{date.AsDateString()}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return _httpClient.GetSingleObjectAsync
            <IDayHistoricalItems<IHistoricalQuote>, JsonDayHistoricalItems<IHistoricalQuote, JsonHistoricalQuoteV1>
            >(
                FakeThrottler.Instance, builder, cancellationToken);
        }
    }
}