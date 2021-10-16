using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal sealed class AlpacaDataClient : IAlpacaDataClient
    {
        private readonly HttpClient _httpClient;

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
            _httpClient.DefaultRequestHeaders.AcceptEncoding
                .Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.BaseAddress = new UriBuilder(
                    configuration.ApiEndpoint) { Path = "v2/stocks/" }.Uri;
            _httpClient.SetSecurityProtocol();
        }

        public void Dispose() => _httpClient.Dispose();

        public Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalBarsAsync(request, cancellationToken)
                : getHistoricalBarsAsync(request, cancellationToken).AsPageAsync<IBar, JsonBarsPage>();

        public Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalBarsAsync(request, cancellationToken).AsMultiPageAsync<IBar, JsonMultiBarsPage>()
                : getHistoricalBarsAsync(request, cancellationToken);

        public Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalQuotesAsync(request, cancellationToken)
                : getHistoricalQuotesAsync(request, cancellationToken)
                    .AsPageAsync<IQuote, JsonQuotesPage<JsonHistoricalQuote>>();

        public Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalQuotesAsync(request, cancellationToken)
                    .AsMultiPageAsync<IQuote, JsonMultiQuotesPage<JsonHistoricalQuote>>()
                : getHistoricalQuotesAsync(request, cancellationToken);

        public Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalTradesAsync(request, cancellationToken)
                : getHistoricalTradesAsync(request, cancellationToken).AsPageAsync<ITrade, JsonTradesPage>();

        public Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? listHistoricalTradesAsync(request, cancellationToken).AsMultiPageAsync<ITrade, JsonMultiTradesPage>()
                : getHistoricalTradesAsync(request, cancellationToken);

        public Task<ITrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ITrade, JsonLatestTrade>(
                $"{symbol.EnsureNotNull(nameof(symbol))}/trades/latest",
                cancellationToken);

        public Task<IQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalQuote>>(
                $"{symbol.EnsureNotNull(nameof(symbol))}/quotes/latest",
                cancellationToken);

        public Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<ISnapshot, JsonSnapshot>(
                $"{symbol.EnsureNotNull(nameof(symbol))}/snapshot", cancellationToken);

        public async Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Query = await new QueryBuilder()
                        .AddParameter("symbols", String.Join(",",
                            symbols.EnsureNotNull(nameof(symbols))))
                        .AsStringAsync().ConfigureAwait(false)
                }.AppendPath("snapshots"),
                StringComparer.Ordinal, kvp => kvp.Value.WithSymbol(kvp.Key),
                cancellationToken).ConfigureAwait(false);

        public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
                "v2/stocks/meta/exchanges", cancellationToken);

        public Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default) =>
            listConditionsAsync(tape, "trade", cancellationToken);

        public Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default) =>
            listConditionsAsync(tape, "quote", cancellationToken);

        private async Task<IReadOnlyDictionary<String, String>> listConditionsAsync(
            Tape tape,
            String tickType,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Query = await new QueryBuilder()
                        .AddParameter("tape", tape.ToEnumString())
                        .AsStringAsync().ConfigureAwait(false)
                }.AppendPath($"v2/stocks/meta/conditions/{tickType}"),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<IBar>> listHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IBar>, JsonBarsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<IBar>> getHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<IBar>, JsonMultiBarsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<IQuote>> listHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<IQuote>, JsonQuotesPage<JsonHistoricalQuote>>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<IQuote>> getHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<IQuote>, JsonMultiQuotesPage<JsonHistoricalQuote>>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IPage<ITrade>> listHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<ITrade>, JsonTradesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<ITrade>> getHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<ITrade>, JsonMultiTradesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
    }
}
