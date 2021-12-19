using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class AlpacaCryptoDataClient : IAlpacaCryptoDataClient
    {
        private readonly HttpClient _httpClient;

        public AlpacaCryptoDataClient(
            AlpacaCryptoDataClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _httpClient = configuration.HttpClient ??
                          configuration.ThrottleParameters.GetHttpClient();

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);
            _httpClient.Configure(new UriBuilder(
                configuration.ApiEndpoint) { Path = "v1beta1/crypto/" }.Uri);
        }

        public void Dispose() => _httpClient.Dispose();

        public Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalBarsAsync(request, cancellationToken)
                : getHistoricalBarsAsync(request, cancellationToken).AsPageAsync<IBar, JsonBarsPage>();

        public Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalBarsAsync(request, cancellationToken).AsMultiPageAsync<IBar, JsonMultiBarsPage>()
                : getHistoricalBarsAsync(request, cancellationToken);

        public Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalCryptoQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalQuotesAsync(request, cancellationToken)
                : getHistoricalQuotesAsync(request, cancellationToken)
                    .AsPageAsync<IQuote, JsonQuotesPage<JsonHistoricalCryptoQuote>>();

        public Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
            HistoricalCryptoQuotesRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalQuotesAsync(request, cancellationToken)
                    .AsMultiPageAsync<IQuote, JsonMultiQuotesPage<JsonHistoricalCryptoQuote>>()
                : getHistoricalQuotesAsync(request, cancellationToken);
        
        public Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalCryptoTradesRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalTradesAsync(request, cancellationToken)
                : getHistoricalTradesAsync(request, cancellationToken).AsPageAsync<ITrade, JsonTradesPage>();

        public Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
            HistoricalCryptoTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalTradesAsync(request, cancellationToken).AsMultiPageAsync<ITrade, JsonMultiTradesPage>()
                : getHistoricalTradesAsync(request, cancellationToken);
        
        public async Task<ITrade> GetLatestTradeAsync(
            LatestDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<ITrade, JsonLatestTrade>(
                await request.EnsureNotNull(nameof(request))
                    .GetUriBuilderAsync(_httpClient, "trades").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public async Task<IQuote> GetLatestQuoteAsync(
            LatestDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalCryptoQuote>>(
                await request.EnsureNotNull(nameof(request))
                    .GetUriBuilderAsync(_httpClient, "quotes").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public async Task<IQuote> GetLatestBestBidOfferAsync(
            LatestBestBidOfferRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IQuote, JsonLatestBestBidOffer>(
                await request.EnsureNotNull(nameof(request))
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<IBar>> listHistoricalBarsAsync(
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IBar>, JsonBarsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<IBar>> getHistoricalBarsAsync(
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<IBar>, JsonMultiBarsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<IQuote>> listHistoricalQuotesAsync(
            HistoricalCryptoQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IQuote>, JsonQuotesPage<JsonHistoricalCryptoQuote>>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<IQuote>> getHistoricalQuotesAsync(
            HistoricalCryptoQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<IQuote>, JsonMultiQuotesPage<JsonHistoricalCryptoQuote>>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<ITrade>> listHistoricalTradesAsync(
            HistoricalCryptoTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<ITrade>, JsonTradesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<ITrade>> getHistoricalTradesAsync(
            HistoricalCryptoTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<ITrade>, JsonMultiTradesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
    }
}
