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

            _httpClient = configuration.HttpClient ?? new HttpClient();

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
                StringComparer.Ordinal, cancellationToken);

        /// <inheritdoc />
        public Task<ILastTrade> GetLastTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastTrade, JsonLastTradeAlpaca>(
                $"v1/last/stocks/{symbol}", cancellationToken);

        /// <inheritdoc />
        public Task<ILastQuote> GetLastQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ILastQuote, JsonLastQuoteAlpaca>(
                $"v1/last_quote/stocks/{symbol}", cancellationToken);
    }
}
