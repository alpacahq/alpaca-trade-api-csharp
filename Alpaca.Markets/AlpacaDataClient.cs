using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
    /// </summary>
    public sealed class AlpacaDataClient : IAlpacaDataClient
    {
        private readonly HttpClient _httpClient;

        private readonly IThrottler _alpacaRestApiThrottler;

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

            _httpClient = configuration.HttpClient ?? new HttpClient();

            _alpacaRestApiThrottler = configuration.ThrottleParameters.GetThrottler();

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IHistoricalBar>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, IReadOnlyList<IHistoricalBar>, String, List<JsonHistoricalBar.V1>>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                StringComparer.Ordinal, cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTrade>(
                $"v1/last/stocks/{symbol}", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuote>(
                $"v1/last_quote/stocks/{symbol}", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<IHistoricalBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IHistoricalBar>, JsonBarsPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<IHistoricalQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IHistoricalQuote>, JsonQuotesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<IHistoricalTrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IHistoricalTrade>, JsonTradesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);
    }
}
