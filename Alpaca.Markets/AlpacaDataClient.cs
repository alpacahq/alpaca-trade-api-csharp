using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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
            _httpClient.Configure(new UriBuilder(
                configuration.ApiEndpoint) { Path = "v2/stocks/" }.Uri);
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

        public Task<IBar> GetLatestBarAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            GetLatestBarAsync(new LatestMarketDataRequest(symbol), cancellationToken);

        public async Task<IBar> GetLatestBarAsync(
            LatestMarketDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IBar, JsonLatestBar>(
                await request.Validate().GetUriBuilderAsync(_httpClient, "bars/latest").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            ListLatestBarsAsync(new LatestMarketDataListRequest(symbols), cancellationToken);

        public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
            LatestMarketDataListRequest request,
            CancellationToken cancellationToken = default) =>
            getLatestAsync<IBar, JsonHistoricalBar>(
                request, "bars/latest", _ => _.Bars, cancellationToken);

        public Task<ITrade> GetLatestTradeAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            GetLatestTradeAsync(new LatestMarketDataRequest(symbol), cancellationToken);

        public async Task<ITrade> GetLatestTradeAsync(
            LatestMarketDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<ITrade, JsonLatestTrade>(
                await request.Validate().GetUriBuilderAsync(_httpClient, "trades/latest").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            ListLatestTradesAsync(new LatestMarketDataListRequest(symbols), cancellationToken);

        public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
            LatestMarketDataListRequest request,
            CancellationToken cancellationToken = default) =>
            getLatestAsync<ITrade, JsonHistoricalTrade>(
                request, "trades/latest", _ => _.Trades, cancellationToken);

        public Task<IQuote> GetLatestQuoteAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            GetLatestQuoteAsync(new LatestMarketDataRequest(symbol), cancellationToken);

        public async Task<IQuote> GetLatestQuoteAsync(
            LatestMarketDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalQuote>>(
                await request.Validate().GetUriBuilderAsync(_httpClient, "quotes/latest").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            ListLatestQuotesAsync(new LatestMarketDataListRequest(symbols), cancellationToken);

        public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
            LatestMarketDataListRequest request,
            CancellationToken cancellationToken = default) =>
            getLatestAsync<IQuote, JsonHistoricalQuote>(
                request, "quotes/latest", _ => _.Quotes, cancellationToken);

        public Task<ISnapshot> GetSnapshotAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            GetSnapshotAsync(new LatestMarketDataRequest(symbol), cancellationToken);

        public async Task<ISnapshot> GetSnapshotAsync(
            LatestMarketDataRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<ISnapshot, JsonSnapshot>(
                await request.GetUriBuilderAsync(_httpClient, "snapshot").ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

        public Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            ListSnapshotsAsync(new LatestMarketDataListRequest(symbols), cancellationToken);

        public async Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
            LatestMarketDataListRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
                await request.Validate().GetUriBuilderAsync(_httpClient, "snapshots").ConfigureAwait(false),
                StringComparer.Ordinal, withSymbol<ISnapshot, JsonSnapshot>,
                cancellationToken).ConfigureAwait(false);

        [ExcludeFromCodeCoverage]
        public Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
            IEnumerable<String> symbols,
            CancellationToken cancellationToken = default) =>
            ListSnapshotsAsync(symbols, cancellationToken);

        public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
                "meta/exchanges", cancellationToken);

        public Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default) =>
            listConditionsAsync(tape, "trade", cancellationToken);

        public Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
            Tape tape,
            CancellationToken cancellationToken = default) =>
            listConditionsAsync(tape, "quote", cancellationToken);

        public async Task<IPage<INewsArticle>> ListNewsArticlesAsync(
            NewsArticlesRequest request,
            CancellationToken cancellationToken = default) =>
            await _httpClient.GetAsync<IPage<INewsArticle>, JsonNewsPage>(
                await request.EnsureNotNull(nameof(request)).Validate()
                    .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);

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
                }.AppendPath($"meta/conditions/{tickType}"),
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

        private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
            LatestMarketDataListRequest request,
            String lastPathSegment,
            Func<JsonLatestData<JsonHistoricalQuote>, Dictionary<String, TJson>> itemsSelector,
            CancellationToken cancellationToken)
            where TJson : TApi, ISymbolMutable =>
            await _httpClient.GetAsync(
                await request.Validate().GetUriBuilderAsync(_httpClient, lastPathSegment).ConfigureAwait(false),
                itemsSelector, withSymbol<TApi, TJson>,
                cancellationToken).ConfigureAwait(false);

        private static TApi withSymbol<TApi, TJson>(
            KeyValuePair<String, TJson> kvp)
            where TJson : TApi, ISymbolMutable
        {
            kvp.Value.SetSymbol(kvp.Key);
            return kvp.Value;
        }
    }
}
