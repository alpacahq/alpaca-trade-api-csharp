namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    private static readonly HttpMethod _httpMethodPatch =
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
            HttpMethod.Patch;
#else
            new("PATCH");
#endif

    public static Task<TApi> PatchAsync<TApi, TJson, TRequest>(
        this HttpClient httpClient,
        String endpointUri,
        TRequest request,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson, TRequest>(
            httpClient, _httpMethodPatch, asUri(endpointUri), request, rateLimitHandler, cancellationToken);
}
