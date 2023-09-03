using System.Net.Http.Headers;

namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    private static readonly String _sdkVersion =
        typeof(HttpClientExtensions).Assembly.GetName().Version!.ToString();

    private static readonly Version _httpVersion =
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        System.Net.HttpVersion.Version20;
#elif NETFRAMEWORK
        new(2, 0);
#else
        System.Net.HttpVersion.Version11;
#endif

    public static HttpClient Configure(
        this HttpClient httpClient,
        SecurityKey securityKey,
        Uri baseAddress)
    {
        foreach (var (header, value) in securityKey.GetAuthenticationHeaders())
        {
            httpClient.DefaultRequestHeaders.Add(header, value);
        }

        httpClient.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.AcceptEncoding
            .Add(new StringWithQualityHeaderValue("gzip"));
        httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue("Alpaca-dotNET-SDK", _sdkVersion));
        httpClient.BaseAddress = baseAddress;

#if NETFRAMEWORK
        // ReSharper disable once StringLiteralTypo
        AppContext.SetSwitch("Switch.System.Net.DontEnableSystemDefaultTlsVersions", false);
#endif

        return httpClient;
    }

    private static async Task<TApi> callAndDeserializeAsync<TApi, TJson>(
        HttpClient httpClient,
        HttpMethod method,
        Uri endpointUri,
        TimeSpan timeout,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi
    {
        using var request = new HttpRequestMessage(method, endpointUri);
        if (timeout != Timeout.InfiniteTimeSpan)
        {
#if NET6_0_OR_GREATER
            request.Options.Set(ThrottleParameters.RequestTimeoutOptionKey, timeout);
#else
            request.Properties[ThrottleParameters.RequestTimeoutOptionKey] = timeout;
#endif
        }
        return await callAndDeserializeAsync<TApi, TJson>(
                httpClient, request, rateLimitHandler, cancellationToken)
            .ConfigureAwait(false);
    }

    private static async Task<TApi> callAndDeserializeAsync<TApi, TJson>(
        HttpClient httpClient,
        HttpMethod method,
        Uri endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi
    {
        using var request = new HttpRequestMessage(method, endpointUri);
        return await callAndDeserializeAsync<TApi, TJson>(
            httpClient, request, rateLimitHandler, cancellationToken)
            .ConfigureAwait(false);
    }

    private static async Task<TApi> callAndDeserializeAsync<TApi, TJson, TContent>(
        HttpClient httpClient,
        HttpMethod method,
        Uri endpointUri,
        TContent content,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi
    {
        using var request = new HttpRequestMessage(method, endpointUri);
        request.Content = toStringContent(content);
        return await callAndDeserializeAsync<TApi, TJson>(
            httpClient, request, rateLimitHandler, cancellationToken)
            .ConfigureAwait(false);
    }

    private static async Task<TApi> callAndDeserializeAsync<TApi, TJson>(
        HttpClient httpClient,
        HttpRequestMessage request,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
        where TJson : TApi
    {
        request.Version = _httpVersion;
        using var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        rateLimitHandler.TryUpdate(response.Headers);
        return await response.DeserializeAsync<TApi, TJson>()
            .ConfigureAwait(false);
    }

    private static async Task<Boolean> callAndReturnSuccessCodeAsync(
        HttpClient httpClient,
        HttpMethod method,
        Uri endpointUri,
        RateLimitHandler rateLimitHandler,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, endpointUri);

        using var response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        rateLimitHandler.TryUpdate(response.Headers);
        return await response.IsSuccessStatusCodeAsync()
            .ConfigureAwait(false);
    }

    private static Uri asUri(String endpointUri) => new(endpointUri, UriKind.RelativeOrAbsolute);

    private static StringContent toStringContent<T>(T value)
    {
        var serializer = new JsonSerializer();
        using var stringWriter = new StringWriter();

        serializer.Serialize(stringWriter, value);
        return new StringContent(stringWriter.ToString());
    }
}
