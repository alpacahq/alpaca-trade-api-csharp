using System;
using System.Collections.Generic;
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

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();

        /// <inheritdoc />
        public async Task<IPage<IBar>> ListHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? await listHistoricalBarsAsync(request, cancellationToken).ConfigureAwait(false)
                : multiPageIntoSinglePage<IBar, JsonBarsPage>(
                    await getHistoricalBarsAsync(request, cancellationToken).ConfigureAwait(false));

        /// <inheritdoc />
        public async Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? singlePageIntoMultiPage<IBar, JsonMultiBarsPage>(
                    await listHistoricalBarsAsync(request, cancellationToken).ConfigureAwait(false))
                : await getHistoricalBarsAsync(request, cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? await listHistoricalQuotesAsync(request, cancellationToken).ConfigureAwait(false)
                : multiPageIntoSinglePage<IQuote, JsonQuotesPage>(
                    await getHistoricalQuotesAsync(request, cancellationToken).ConfigureAwait(false));

        /// <inheritdoc />
        public async Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? singlePageIntoMultiPage<IQuote, JsonMultiQuotesPage>(
                    await listHistoricalQuotesAsync(request, cancellationToken).ConfigureAwait(false))
                : await getHistoricalQuotesAsync(request, cancellationToken).ConfigureAwait(false);

        /// <inheritdoc />
        public async Task<IPage<ITrade>> ListHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? await listHistoricalTradesAsync(request, cancellationToken).ConfigureAwait(false)
                : multiPageIntoSinglePage<ITrade, JsonTradesPage>(
                    await getHistoricalTradesAsync(request, cancellationToken).ConfigureAwait(false));

        /// <inheritdoc />
        public async Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
            HistoricalTradesRequest request, 
            CancellationToken cancellationToken = default) =>
            request.Symbols.Count == 1
                ? singlePageIntoMultiPage<ITrade, JsonMultiTradesPage>(
                    await listHistoricalTradesAsync(request, cancellationToken).ConfigureAwait(false))
                : await getHistoricalTradesAsync(request, cancellationToken).ConfigureAwait(false);

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
            await _httpClient.GetAsync<IPage<IQuote>, JsonQuotesPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        private async Task<IMultiPage<IQuote>> getHistoricalQuotesAsync(
            HistoricalQuotesRequest request, 
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IMultiPage<IQuote>, JsonMultiQuotesPage>(
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

        private static IPage<TItem> multiPageIntoSinglePage<TItem, TPage>(
            IMultiPage<TItem> response) where TPage : IPageMutable<TItem>, new() =>
            new TPage
            {
                Items = new List<TItem>(response.Items.SelectMany(_ => _.Value)),
                NextPageToken = response.NextPageToken
            };

        private static IMultiPage<TItem> singlePageIntoMultiPage<TItem, TPage>(
            IPage<TItem> response) where TPage : IMultiPageMutable<TItem>, new() =>
            new TPage
            {
                Items = new Dictionary<String, IReadOnlyList<TItem>>
                {
                    { response.Symbol, response.Items }
                },
                NextPageToken = response.NextPageToken
            };
    }
}
