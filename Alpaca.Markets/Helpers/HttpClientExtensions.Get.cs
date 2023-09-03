namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    public static Task<TApi> GetAsync<TApi, TJson>(
        this HttpClient httpClient,
        UriBuilder uriBuilder,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson>(
            httpClient, HttpMethod.Get, uriBuilder.Uri, rateLimitHandler, cancellationToken);

    public static Task<TApi> GetAsync<TApi, TJson>(
        this HttpClient httpClient,
        String endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson>(
            httpClient, HttpMethod.Get, asUri(endpointUri), rateLimitHandler, cancellationToken);

    public static async Task<IReadOnlyDictionary<TKeyApi, TValueApi>> GetAsync
        <TKeyApi, TValueApi, TKeyJson, TValueJson>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            IEqualityComparer<TKeyApi> comparer,
            Func<KeyValuePair<TKeyJson, TValueJson>, TValueApi> elementSelector,
            RateLimitHandler rateLimitHandler,
            CancellationToken cancellationToken)
        where TKeyApi : notnull
        where TKeyJson : TKeyApi
        where TValueJson : TValueApi =>
        getReadOnlyDictionary(await httpClient
            .GetAsync<Dictionary<TKeyJson, TValueJson>, Dictionary<TKeyJson, TValueJson>>(
                uriBuilder, rateLimitHandler, cancellationToken)
            .ConfigureAwait(false), elementSelector, comparer);

    public static async Task<IReadOnlyDictionary<String, TValueApi>> GetAsync
        <TValueApi, TValueJson, TStorage>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            Func<TStorage, Dictionary<String, TValueJson>> itemsSelector,
            Func<KeyValuePair<String, TValueJson>, TValueApi> elementSelector,
            RateLimitHandler rateLimitHandler,
            CancellationToken cancellationToken)
        where TValueJson : TValueApi, ISymbolMutable =>
        // ReSharper disable once RedundantTypeArgumentsOfMethod
        getReadOnlyDictionary<String, TValueApi, String, TValueJson>(
            itemsSelector(await httpClient.GetAsync<TStorage, TStorage>(
                    uriBuilder, rateLimitHandler, cancellationToken)
                .ConfigureAwait(false)), elementSelector, StringComparer.Ordinal);

    private static IReadOnlyDictionary<TKeyApi, TValueApi> getReadOnlyDictionary<TKeyApi, TValueApi, TKeyJson, TValueJson>(
        Dictionary<TKeyJson, TValueJson> response, 
        Func<KeyValuePair<TKeyJson, TValueJson>, TValueApi> elementSelector,
        IEqualityComparer<TKeyApi> comparer)
        where TKeyApi : notnull
        where TKeyJson : TKeyApi
        where TValueJson : TValueApi =>
        response
            .Where(pair => pair.Value is not null)
            .ToDictionary(pair => pair.Key, elementSelector, comparer);
}
