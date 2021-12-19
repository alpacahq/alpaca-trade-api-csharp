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
    internal sealed class AlpacaDataClient : IAlpacaDataClient
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
            _httpClient.Configure(new UriBuilder(
                configuration.ApiEndpoint) { Path = "v2/stocks/" }.Uri);
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <inheritdoc />
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default) =>
            throw new NotImplementedException(
                "This Alpaca Data API v1 endpoint will be deprecated soon.");

        /// <inheritdoc />
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            throw new NotImplementedException(
                "This Alpaca Data API v1 endpoint will be deprecated soon.");

        /// <inheritdoc />
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            throw new NotImplementedException(
                "This Alpaca Data API v1 endpoint will be deprecated soon.");

        /// <inheritdoc />
        public async Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IBar>, JsonBarsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IQuote>, JsonQuotesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<ITrade>, JsonTradesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public Task<ITrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ITrade, JsonLatestTrade>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/trades/latest",
                cancellationToken);

        /// <inheritdoc />
        public Task<IQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IQuote, JsonLatestQuote>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/quotes/latest",
                cancellationToken);

        /// <inheritdoc />
        public Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ISnapshot, JsonSnapshot>(
                $"v2/stocks/{symbol.EnsureNotNull(nameof(symbol))}/snapshot", cancellationToken);

        /// <inheritdoc />
        public async Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Path = "v2/stocks/snapshots",
                    Query = await new QueryBuilder()
                        .AddParameter("symbols", String.Join(",",
                            symbols.EnsureNotNull(nameof(symbols))))
                        .AsStringAsync().ConfigureAwait(false)
                },
                StringComparer.Ordinal, kvp => kvp.Value.WithSymbol(kvp.Key),
                cancellationToken).ConfigureAwait(false);
    }
}
