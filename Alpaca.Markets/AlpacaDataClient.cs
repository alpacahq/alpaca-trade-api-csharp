﻿namespace Alpaca.Markets;

internal sealed class AlpacaDataClient :
    DataHistoricalClientBase<HistoricalBarsRequest, HistoricalQuotesRequest, JsonHistoricalQuote, HistoricalTradesRequest>,
    IAlpacaDataClient
{
    internal AlpacaDataClient(
        AlpacaDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
    {
    }

    public async Task<IBar> GetLatestBarAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IBar, JsonLatestBar>(
            await request.Validate().GetUriBuilderAsync(HttpClient, "bars/latest").ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IBar, JsonHistoricalBar>(
            request, "bars/latest", _ => _.Bars, cancellationToken);

    public async Task<ITrade> GetLatestTradeAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            await request.Validate().GetUriBuilderAsync(HttpClient, "trades/latest").ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonHistoricalTrade>(
            request, "trades/latest", _ => _.Trades, cancellationToken);

    public async Task<IQuote> GetLatestQuoteAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalQuote>>(
            await request.Validate().GetUriBuilderAsync(HttpClient, "quotes/latest").ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonHistoricalQuote>(
            request, "quotes/latest", _ => _.Quotes, cancellationToken);

    public async Task<ISnapshot> GetSnapshotAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ISnapshot, JsonSnapshot>(
            await request.Validate().GetUriBuilderAsync(HttpClient, "snapshot").ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
            await request.Validate().GetUriBuilderAsync(HttpClient, "snapshots").ConfigureAwait(false),
            StringComparer.Ordinal, withSymbol<ISnapshot, JsonSnapshot>,
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            "meta/exchanges", RateLimitHandler, cancellationToken);

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
        await HttpClient.GetAsync<IPage<INewsArticle>, JsonNewsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, String>> listConditionsAsync(
        Tape tape,
        String tickType,
        CancellationToken cancellationToken) =>
        await HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            new UriBuilder(HttpClient.BaseAddress!)
            {
                Query = await new QueryBuilder()
                    .AddParameter("tape", tape.ToEnumString())
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath($"meta/conditions/{tickType}"),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        LatestMarketDataListRequest request,
        String lastPathSegment,
        Func<JsonLatestData<JsonHistoricalQuote>, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await HttpClient.GetAsync(
            await request.Validate().GetUriBuilderAsync(HttpClient, lastPathSegment).ConfigureAwait(false),
            itemsSelector, withSymbol<TApi, TJson>,
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
