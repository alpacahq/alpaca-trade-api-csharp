namespace Alpaca.Markets;

internal static class ActiveStocksExtensions
{
    public static async Task<IReadOnlyList<IActiveStock>> ListMostActiveStocksAsync(
        this HttpClient httpClient,
        RateLimitHandler rateLimitHandler,
        String orderByFieldForRankingMostActive,
        Int32? numberOfTopMostActiveStocks = default,
        CancellationToken cancellationToken = default) =>
        (await httpClient.GetAsync<JsonActiveStocks, JsonActiveStocks>(
            await getUriBuilderAsync(httpClient, orderByFieldForRankingMostActive, numberOfTopMostActiveStocks).ConfigureAwait(false),
            rateLimitHandler, cancellationToken).ConfigureAwait(false))
        .MostActives.EmptyIfNull<IActiveStock>();

    private static async ValueTask<UriBuilder> getUriBuilderAsync(
        HttpClient httpClient,
        String orderByField,
        Int32? top = default) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("by", orderByField)
                .AddParameter("top", top)
                .AsStringAsync().ConfigureAwait(false)
        }.WithPath("v1beta1/screener/stocks/most-actives");
}
