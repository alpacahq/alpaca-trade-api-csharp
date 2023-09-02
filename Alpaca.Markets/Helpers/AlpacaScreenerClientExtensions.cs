namespace Alpaca.Markets;

internal static class AlpacaScreenerClientExtensions
{
    public static async Task<IMarketMovers> GetTopMarketMoversAsync(
        this HttpClient httpClient,
        RateLimitHandler rateLimitHandler,
        String marketType,
        Int32? numberOfLosersAndGainersInResponse = default,
        CancellationToken cancellationToken = default) =>
        await httpClient.GetAsync<IMarketMovers, JsonMarketMovers>(
            await getUriBuilderAsync(httpClient, marketType, numberOfLosersAndGainersInResponse).ConfigureAwait(false),
            rateLimitHandler, cancellationToken).ConfigureAwait(false);

    private static async ValueTask<UriBuilder> getUriBuilderAsync(
        HttpClient httpClient,
        String marketType,
        Int32? top = default) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("top", top)
                .AsStringAsync().ConfigureAwait(false)
        }.WithPath($"v1beta1/screener/{marketType}/movers");
}
