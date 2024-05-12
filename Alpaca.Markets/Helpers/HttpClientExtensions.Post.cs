namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    public static Task<Boolean> TryPostAsync(
        this HttpClient httpClient,
        String endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken) =>
        callAndReturnSuccessCodeAsync(
            httpClient, HttpMethod.Post, asUri(endpointUri), rateLimitHandler, cancellationToken);

    public static Task<TApi> PostAsync<TApi, TJson, TRequest>(
        this HttpClient httpClient,
        String endpointUri,
        TRequest request,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson, TRequest>(
            httpClient, HttpMethod.Post, asUri(endpointUri), request, rateLimitHandler, cancellationToken);

    public static Task<TApi> PostAsync<TApi, TJson, TRequest>(
        this HttpClient httpClient,
        UriBuilder uriBuilder,
        TRequest request,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson, TRequest>(
            httpClient, HttpMethod.Post, uriBuilder.Uri, request, rateLimitHandler, cancellationToken);

    public static Task<TApi> PutAsync<TApi, TJson, TRequest>(
        this HttpClient httpClient,
        String endpointUri,
        TRequest request,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson, TRequest>(
            httpClient, HttpMethod.Put, asUri(endpointUri), request, rateLimitHandler, cancellationToken);

}
