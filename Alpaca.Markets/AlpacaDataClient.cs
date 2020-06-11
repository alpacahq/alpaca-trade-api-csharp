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

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical daily bars request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public Task<IReadOnlyDictionary<String, IReadOnlyList<IAgg>>> GetBarSetAsync(
            BarSetRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<String, IReadOnlyList<IAgg>, String, List<JsonAlpacaAgg>>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient),
                StringComparer.Ordinal, cancellationToken);

        /// <summary>
        /// Gets last trade for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only last trade information.</returns>
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTradeAlpaca>(
                $"v1/last/stocks/{symbol}", cancellationToken);

        /// <summary>
        /// Gets current quote for singe asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only current quote information.</returns>
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuoteAlpaca>(
                $"v1/last_quote/stocks/{symbol}", cancellationToken);
    }
}
