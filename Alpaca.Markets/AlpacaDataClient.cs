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
    [Obsolete("This class will be marked as internal in the next major SDK release.", false)]
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
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, IReadOnlyList<IAgg>, String, List<JsonAlpacaAgg>>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                StringComparer.Ordinal, _ => _.Value,
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTradeAlpaca>(
                $"v1/last/stocks/{symbol.EnsureNotNull(nameof(symbol))}", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuoteAlpaca>(
                $"v1/last_quote/stocks/{symbol.EnsureNotNull(nameof(symbol))}", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IPage<IAgg>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IAgg>, JsonBarsPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IPage<IHistoricalQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IHistoricalQuote>, JsonQuotesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IPage<IHistoricalTrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IHistoricalTrade>, JsonTradesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IStreamTrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IStreamTrade, JsonLatestTrade>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/trades/latest", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IStreamQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IStreamQuote, JsonLatestQuote>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/quotes/latest", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ISnapshot, JsonSnapshot>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/snapshot", cancellationToken,
                _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
                new UriBuilder(_httpClient.BaseAddress)
                {
                    Path = "v2/stocks/snapshots",
                    Query = new QueryBuilder()
                        .AddParameter("symbols", String.Join(",", symbols))
                },
                StringComparer.Ordinal, kvp => kvp.Value.WithSymbol(kvp.Key),
                cancellationToken, _alpacaRestApiThrottler);
    }
}
