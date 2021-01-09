using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon Data API via HTTP/REST.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class PolygonDataClient : IPolygonDataClient
    {
        private readonly HttpClient _httpClient;

        private readonly Boolean _isStagingEnvironment;

        private readonly String _keyId;

        /// <summary>
        /// Creates new instance of <see cref="PolygonDataClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public PolygonDataClient(
            PolygonDataClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _httpClient = configuration.HttpClient ?? new HttpClient();

            _isStagingEnvironment = configuration.KeyId
                .EndsWith("-staging", StringComparison.Ordinal);
            _keyId = configuration.KeyId;

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <inheritdoc />
        public Task<IReadOnlyList<IExchange>> ListExchangesAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IExchange>, List<JsonExchange>>(
                GetUriBuilder("v1/meta/exchanges"), cancellationToken);

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<String, String>> GetSymbolTypeMapAsync(
            CancellationToken cancellationToken = default)
        {
            var map = await _httpClient.GetAsync<JsonSymbolTypeMap, JsonSymbolTypeMap>(
                    GetUriBuilder("v2/reference/types"), cancellationToken)
                .ConfigureAwait(false);

            return map.Results.StockTypes
                .Concat(map.Results.IndexTypes)
                .GroupBy(
                    kvp => kvp.Key, 
                    kvp => kvp.Value,
                    StringComparer.Ordinal)
                .ToDictionary(
                    group => group.Key,
                    group => group.First(),
                    StringComparer.Ordinal);
        }

        /// <inheritdoc />
        public Task<IHistoricalItems<IHistoricalTrade>> ListHistoricalTradesAsync(
            HistoricalRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync
                <IHistoricalItems<IHistoricalTrade>, JsonHistoricalItems<IHistoricalTrade, JsonHistoricalTrade>>(
                    request.EnsureNotNull(nameof(request)).Validate()
                        .GetUriBuilder(this, "trades"),
                    cancellationToken);

        /// <inheritdoc />
        public Task<IHistoricalItems<IHistoricalQuote>> ListHistoricalQuotesAsync(
            HistoricalRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync
                <IHistoricalItems<IHistoricalQuote>, JsonHistoricalItems<IHistoricalQuote, JsonHistoricalQuote>>(
                    request.EnsureNotNull(nameof(request)).Validate()
                        .GetUriBuilder(this, "nbbo"),
                    cancellationToken);

        /// <inheritdoc />
        public Task<IHistoricalItems<IAgg>> ListAggregatesAsync(
            AggregatesRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync
                <IHistoricalItems<IAgg>, JsonHistoricalItems<IAgg, JsonPolygonAgg>>(
                    request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(this), 
                    cancellationToken);

        /// <inheritdoc />
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTradePolygon>(
                GetUriBuilder($"v1/last/stocks/{symbol}"), cancellationToken);

        /// <inheritdoc />
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuotePolygon>(
                GetUriBuilder($"v1/last_quote/stocks/{symbol}"), cancellationToken);

        /// <inheritdoc />
        public Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            CancellationToken cancellationToken = default) =>
            GetConditionMapAsync(TickType.Trades, cancellationToken);

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<Int64, String>> GetConditionMapAsync(
            TickType tickType,
            CancellationToken cancellationToken = default)
        {
            var dictionary = await _httpClient.GetAsync
                    <IDictionary<String, String>, Dictionary<String, String>>(
                        GetUriBuilder($"v1/meta/conditions/{tickType.ToEnumString()}"),
                        cancellationToken)
                .ConfigureAwait(false);

            return dictionary
                .ToDictionary(
                    kvp => Int64.Parse(kvp.Key,
                        NumberStyles.Integer, CultureInfo.InvariantCulture),
                    kvp => kvp.Value);
        }

        internal PolygonUriBuilder GetUriBuilder(
            String path)
            => new PolygonUriBuilder(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Path = path
                }, 
                getDefaultPolygonApiQueryBuilder());

        private QueryBuilder getDefaultPolygonApiQueryBuilder()
        {
            var builder = new QueryBuilder()
                .AddParameter("apiKey", _keyId);

            if (_isStagingEnvironment)
            {
                builder.AddParameter("staging", "true");
            }

            return builder;
        }
    }
}
