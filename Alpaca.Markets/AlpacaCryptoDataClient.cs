namespace Alpaca.Markets;

using JsonLatestData = JsonLatestData<JsonHistoricalCryptoQuote, JsonHistoricalTrade, JsonCryptoSnapshot>;

internal sealed class AlpacaCryptoDataClient :
    DataHistoricalClientBase<
        HistoricalCryptoBarsRequest,
        HistoricalCryptoQuotesRequest, JsonHistoricalCryptoQuote,
        HistoricalCryptoTradesRequest, JsonHistoricalTrade>,
    IAlpacaCryptoDataClient
{
    internal AlpacaCryptoDataClient(
        AlpacaCryptoDataClientConfiguration configuration)
#pragma warning disable CA2000
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
#pragma warning restore CA2000
    {
    }

    public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IBar, JsonHistoricalBar>(
            request, "bars", data => data.Bars, cancellationToken);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonHistoricalTrade>(
            request, "trades", data => data.Trades, cancellationToken);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonHistoricalCryptoQuote>(
            request, "quotes", data => data.Quotes, cancellationToken);

    public async Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        SnapshotDataListRequest request,
        CancellationToken cancellationToken = default) =>
        await getLatestAsync<ISnapshot, JsonCryptoSnapshot>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            data => data.Snapshots, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyDictionary<String, IOrderBook>> ListLatestOrderBooksAsync(
        LatestOrderBooksRequest request,
        CancellationToken cancellationToken = default) =>
        await getLatestAsync<IOrderBook, JsonHistoricalOrderBook>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            data => data.OrderBooks, cancellationToken).ConfigureAwait(false);

    public Task<IMarketMovers> GetTopMarketMoversAsync(
        Int32? numberOfLosersAndGainersInResponse = default,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetTopMarketMoversAsync(RateLimitHandler,
            "crypto", numberOfLosersAndGainersInResponse, cancellationToken);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        LatestDataListRequest request,
        String items,
        Func<JsonLatestData, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await getLatestAsync<TApi, TJson>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, items).ConfigureAwait(false),
            itemsSelector, cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        UriBuilder uriBuilder,
        Func<JsonLatestData, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await HttpClient.GetAsync(
            uriBuilder, itemsSelector, withSymbol<TApi, TJson>,
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
