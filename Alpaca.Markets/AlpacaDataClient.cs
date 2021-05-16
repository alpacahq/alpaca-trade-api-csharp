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

            _httpClient = configuration.HttpClient ??
                          configuration.ThrottleParameters.GetHttpClient();

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
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, IReadOnlyList<IBar>, String, List<JsonHistoricalBar.V1>>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                StringComparer.Ordinal, kvp =>
                {
                    kvp.Value.ForEach(_ => _.Symbol = kvp.Key);
                    return kvp.Value;
                },
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTrade>(
                $"v1/last/stocks/{symbol.EnsureNotNull(nameof(symbol))}", cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuote>(
                $"v1/last_quote/stocks/{symbol.EnsureNotNull(nameof(symbol))}", cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IBar>, JsonBarsPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<IQuote>, JsonQuotesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPage<ITrade>, JsonTradesPage>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ITrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ITrade, JsonLatestTrade>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/trades/latest",
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IQuote, JsonLatestQuote>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/quotes/latest",
                cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ISnapshot, JsonSnapshot>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/snapshot", cancellationToken);

        /// <inheritdoc />
        [CLSCompliant(false)]
        public Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Path = "v2/stocks/snapshots",
                    Query = new QueryBuilder()
                        .AddParameter("symbols", String.Join(",", symbols))
                },
                StringComparer.Ordinal, kvp => kvp.Value.WithSymbol(kvp.Key),
                cancellationToken);
    }
}
