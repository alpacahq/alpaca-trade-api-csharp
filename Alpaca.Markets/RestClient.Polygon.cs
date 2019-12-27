using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        /// <summary>
        /// Gets list of available exchanges from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of exchange information objects.</returns>
        public Task<IReadOnlyList<IExchange>> ListExchangesAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/exchanges",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getObjectsListAsync<IExchange, JsonExchange>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets mapping dictionary for symbol types from Polygon REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to symbol type abbreviation and values
        /// equal to full symbol type names descriptions for each supported symbol type.
        /// </returns>
        public Task<IReadOnlyDictionary<String, String>> GetSymbolTypeMapAsync(
        CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = "v1/meta/symbol-types",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync
                <IReadOnlyDictionary<String,String>, Dictionary<String, String>>(
                    _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
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
        public Task<IHistoricalItems<IHistoricalTrade>> ListHistoricalTradesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v2/ticks/stocks/trades/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("timestamp", timestamp)
                    .AddParameter("timestamp_limit", timestampLimit)
                    .AddParameter("limit", limit)
                    .AddParameter("reverse", reverse != null ? reverse.ToString() : null)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IHistoricalTrade>,
                    JsonHistoricalItems<IHistoricalTrade, JsonHistoricalTrade>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of historical trades for single asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="offset">Paging - offset or first historical trade in days trades list.</param>
        /// <param name="limit">Paging - maximal number of historical trades in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical trade information.</returns>
        [Obsolete("This version of ListHistoricalTradesAsync will be deprecated in a future release.", false)]
        public Task<IDayHistoricalItems<IHistoricalTrade>> ListHistoricalTradesV1Async(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null,
            CancellationToken cancellationToken = default)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/trades/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IDayHistoricalItems<IHistoricalTrade>,
                    JsonDayHistoricalItems<IHistoricalTrade, JsonHistoricalTradeV1>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
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
        public Task<IHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesAsync(
            String symbol,
            DateTime date,
            Int64? timestamp = null,
            Int64? timestampLimit = null,
            Int32? limit = null,
            Boolean? reverse = null,
            CancellationToken cancellationToken = default)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v2/ticks/stocks/nbbo/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("timestamp", timestamp)
                    .AddParameter("timestamp_limit", timestampLimit)
                    .AddParameter("limit", limit)
                    .AddParameter("reverse", reverse != null ? reverse.ToString() : null)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IHistoricalQuote>,
                    JsonHistoricalItems<IHistoricalQuote, JsonHistoricalQuote>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of historical quotes for single asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="date">Single date for data retrieval.</param>
        /// <param name="offset">Paging - offset or first historical quote in days quotes list.</param>
        /// <param name="limit">Paging - maximal number of historical quotes in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical quote information.</returns>
        [Obsolete("This version of ListHistoricalQuotesAsync will be deprecated in a future release.", false)]

        public Task<IDayHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesV1Async(
            String symbol,
            DateTime date,
            Int64? offset = null,
            Int32? limit = null,
            CancellationToken cancellationToken = default)
        {
            var dateAsString = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/historic/quotes/{symbol}/{dateAsString}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("offset", offset)
                    .AddParameter("limit", limit)
            };

            return getSingleObjectAsync
                <IDayHistoricalItems<IHistoricalQuote>,
                    JsonDayHistoricalItems<IHistoricalQuote, JsonHistoricalQuoteV1>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of historical minute bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="multiplier">>Number of bars to combine in each result.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="unadjusted">Set to true if the results should not be adjusted for splits.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of minute bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListMinuteAggregatesAsync(
            String symbol,
            Int32 multiplier,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            Boolean unadjusted = false,
            CancellationToken cancellationToken = default)
        {
            var unixFrom = DateTimeHelper.GetUnixTimeMilliseconds(dateFromInclusive);
            var unixTo = DateTimeHelper.GetUnixTimeMilliseconds(dateToInclusive);

            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v2/aggs/ticker/{symbol}/range/{multiplier}/minute/{unixFrom}/{unixTo}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("unadjusted", unadjusted ? Boolean.TrueString : Boolean.FalseString)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IAgg>,
                    JsonHistoricalItems<IAgg, JsonMinuteAgg>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of historical minute bars for single asset from Polygon's v2 REST API endpoint.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="multiplier">>Number of bars to combine in each result.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="unadjusted">Set to true if the results should not be adjusted for splits.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of day bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListDayAggregatesAsync(
            String symbol,
            Int32 multiplier,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            Boolean unadjusted = false,
            CancellationToken cancellationToken = default)
        {
            var unixFrom = DateTimeHelper.GetUnixTimeMilliseconds(dateFromInclusive);
            var unixTo = DateTimeHelper.GetUnixTimeMilliseconds(dateToInclusive);

            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v2/aggs/ticker/{symbol}/range/{multiplier}/day/{unixFrom}/{unixTo}",
                Query = getDefaultPolygonApiQueryBuilder()
                    .AddParameter("unadjusted", unadjusted ? Boolean.TrueString : Boolean.FalseString)
            };

            return getSingleObjectAsync
                <IHistoricalItems<IAgg>,
                    JsonHistoricalItems<IAgg, JsonMinuteAgg>>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets last trade for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/last/stocks/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync<ILastTrade, JsonLastTrade>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets current quote for singe asset from Polygon REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/last_quote/stocks/{symbol}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            return getSingleObjectAsync<ILastQuote, JsonLastQuote>(
                _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets mapping dictionary for specific tick type from Polygon REST API endpoint.
        /// </summary>
        /// <param name="tickType">Tick type for conditions map.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>
        /// Read-only dictionary with keys equal to condition integer values and values
        /// equal to full tick condition descriptions for each supported tick type.
        /// </returns>
        public async Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            TickType tickType = TickType.Trades,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_polygonHttpClient.BaseAddress)
            {
                Path = $"v1/meta/conditions/{tickType.ToEnumString()}",
                Query = getDefaultPolygonApiQueryBuilder()
            };

            var dictionary = await getSingleObjectAsync
                <IDictionary<String, String>, Dictionary<String, String>>(
                    _polygonHttpClient, FakeThrottler.Instance, builder, cancellationToken)
                .ConfigureAwait(false);

            return dictionary
                .ToDictionary(
                    kvp => Int64.Parse(kvp.Key,
                        NumberStyles.Integer, CultureInfo.InvariantCulture),
                    kvp => kvp.Value);
        }

        private QueryBuilder getDefaultPolygonApiQueryBuilder()
        {
            var builder = new QueryBuilder()
                .AddParameter("apiKey", _configuration.KeyId);

            if (_isPolygonStaging)
            {
                builder.AddParameter("staging", "true");
            }

            return builder;
        }
    }
}
