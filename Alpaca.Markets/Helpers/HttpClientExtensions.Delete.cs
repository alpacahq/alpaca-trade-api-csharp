namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    public static Task<Boolean> TryDeleteAsync(
        this HttpClient httpClient,
        UriBuilder uriBuilder,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken) =>
        callAndReturnSuccessCodeAsync(
            httpClient, HttpMethod.Delete, uriBuilder.Uri, rateLimitHandler, cancellationToken);

    public static Task<Boolean> TryDeleteAsync(
        this HttpClient httpClient,
        String endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken) =>
        callAndReturnSuccessCodeAsync(
            httpClient, HttpMethod.Delete, asUri(endpointUri), rateLimitHandler, cancellationToken);

    public static Task<TApi> DeleteAsync<TApi, TJson>(
        this HttpClient httpClient,
        UriBuilder uriBuilder,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson>(
            httpClient, HttpMethod.Delete, uriBuilder.Uri, rateLimitHandler, cancellationToken);

    public static Task<TApi> DeleteAsync<TApi, TJson>(
        this HttpClient httpClient,
        UriBuilder uriBuilder,
        TimeSpan timeout,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson>(
            httpClient, HttpMethod.Delete, uriBuilder.Uri, timeout, rateLimitHandler, cancellationToken);

    public static Task<TApi> DeleteAsync<TApi, TJson>(
        this HttpClient httpClient,
        String endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi =>
        callAndDeserializeAsync<TApi, TJson>(
            httpClient, HttpMethod.Delete, asUri(endpointUri), rateLimitHandler, cancellationToken);
}
