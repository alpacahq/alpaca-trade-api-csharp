using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
    /// </summary>
    public sealed class AlpacaDataClient : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaDataClient(
            AlpacaDataClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            configuration.SecurityId
                .AddAuthenticationHeader(_httpClient, configuration.KeyId);

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint
                .AddApiVersionNumberSafe(ApiVersion.V1);
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">>Asset names for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="areTimesInclusive">
        /// If <c>true</c> - both <paramref name="timeFrom"/> and <paramref name="timeInto"/> parameters are treated as inclusive.
        /// </param>
        /// <param name="timeFrom">Start time for filtering.</param>
        /// <param name="timeInto">End time for filtering.</param>
        /// <param name="limit">Maximal number of daily bars in data response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        [Obsolete("Use overloaded method that required BarSetRequest parameter instead of this one.")]
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            IEnumerable<String> symbols,
            TimeFrame timeFrame,
            Int32? limit = BarSetRequest.DefaultLimit,
            Boolean areTimesInclusive = true,
            DateTime? timeFrom = null,
            DateTime? timeInto = null,
            CancellationToken cancellationToken = default) =>
            GetBarSetAsync(new BarSetRequest(symbols, timeFrame)
            {
                Limit = limit,
                TimeFrom = timeFrom,
                TimeInto = timeInto,
                AreTimesInclusive = areTimesInclusive,
            }, cancellationToken);

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical daily bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public async Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"bars/{request.TimeFrame.ToEnumString()}",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", request.Symbols))
                    .AddParameter((request.AreTimesInclusive ? "start" : "after"), request.TimeFrom, "O")
                    .AddParameter((request.AreTimesInclusive ? "end" : "until"), request.TimeInto, "O")
                    .AddParameter("limit", request.Limit)
            };

            var response = await _httpClient
                .GetSingleObjectAsync<IReadOnlyDictionary<String, List<JsonBarAgg>>, Dictionary<String, List<JsonBarAgg>>>(
                    FakeThrottler.Instance, builder, cancellationToken)
                .ConfigureAwait(false);

            return response.ToDictionary(
                kvp => kvp.Key, 
                kvp => (IReadOnlyList<IAgg>)kvp.Value.AsReadOnly());
        }

        /// <summary>
        /// Gets list of historical bars for single asset from from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="multiplier">Number of bars to combine in each result.</param>
        /// <param name="timeSpan">Size of the aggregation time window.</param>
        /// <param name="dateFromInclusive">Start time for filtering (inclusive).</param>
        /// <param name="dateToInclusive">End time for filtering (inclusive).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of day bars for specified asset.</returns>
        public Task<IHistoricalItems<IAgg>> ListAggregatesAsync(
            String symbol,
            Int32 multiplier,
            AggregationTimeSpan timeSpan,
            DateTime dateFromInclusive,
            DateTime dateToInclusive,
            CancellationToken cancellationToken = default)
        {
            var dateStringFrom = dateFromInclusive.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var dateStringTo = dateToInclusive.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"aggs/ticker/{symbol}/range/{multiplier}/{timeSpan.ToEnumString()}/{dateStringFrom}/{dateStringTo}",
            };

            return _httpClient.GetSingleObjectAsync<IHistoricalItems<IAgg>, JsonHistoricalItems<IAgg, JsonMinuteAgg>>(
                FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets last trade for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"last/stocks/{symbol}"
            };

            return _httpClient.GetSingleObjectAsync<ILastTrade, JsonLastTrade>(
                FakeThrottler.Instance, builder, cancellationToken);
        }

        /// <summary>
        /// Gets current quote for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"last_quote/stocks/{symbol}"
            };

            return _httpClient.GetSingleObjectAsync<ILastQuote, JsonLastQuote>(
                FakeThrottler.Instance, builder, cancellationToken);
        }
    }
}
