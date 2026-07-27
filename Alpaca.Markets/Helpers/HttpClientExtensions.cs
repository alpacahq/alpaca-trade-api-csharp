using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Alpaca.Markets;

internal static partial class HttpClientExtensions
{
    // Matches a dotted version number (2 to 4 components) anywhere in a runtime
    // description string, e.g. "8.0.7" in ".NET 8.0.7" or "6.12.0.122" in
    // "Mono 6.12.0.122 (...)".
    private static readonly Regex _runtimeVersionPattern = new(
        @"\d+(\.\d+){1,3}", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly String _sdkVersion =
        typeof(HttpClientExtensions).Assembly.GetName().Version!.ToString(3);

    private static readonly (String Name, String Version) _runtimeInfo =
        ParseRuntimeInfo(RuntimeInformation.FrameworkDescription);

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
            new ProductInfoHeaderValue("APCA-DOTNET", _sdkVersion));
        httpClient.DefaultRequestHeaders.UserAgent.Add(
            new ProductInfoHeaderValue(_runtimeInfo.Name, _runtimeInfo.Version));
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

    // Internal (rather than private) so tests can validate parsing of arbitrary/malformed
    // runtime descriptions (e.g. Mono's, whose format is explicitly documented as unstable)
    // without needing to run on every target runtime.
    internal static (String Name, String Version) ParseRuntimeInfo(
        String frameworkDescription)
    {
        // Splits ".NET 8.0.7" into (".NET", "8.0.7") or "Mono 6.12.0.122 (2020-02/8e1b8f4 ...)"
        // into ("Mono", "6.12.0.122") by locating the first dotted version number.
        var match = _runtimeVersionPattern.Match(frameworkDescription);

        var name = sanitizeProductToken(match.Success
            ? frameworkDescription[..match.Index]
            : frameworkDescription);
        var version = match.Success
            ? match.Value
            : Environment.Version.ToString();

        // HTTP product tokens (RFC 7230) can't be empty and can't contain separator
        // characters (spaces, parentheses, slashes, colons, etc.) - strip anything else
        // out so a surprising runtime description can never crash header construction.
        return (String.IsNullOrEmpty(name) ? ".NET" : name, version);
    }

    private static String sanitizeProductToken(
        String value) =>
        new(value.Where(isProductTokenChar).ToArray());

    private static Boolean isProductTokenChar(
        Char character) =>
        Char.IsLetterOrDigit(character) || character is '.' or '-' or '_' or '+';

    private static Uri asUri(String endpointUri) => new(endpointUri, UriKind.RelativeOrAbsolute);

    private static StringContent toStringContent<T>(T value)
    {
        var serializer = new JsonSerializer();
        using var stringWriter = new StringWriter();

        serializer.Serialize(stringWriter, value);
        return new StringContent(stringWriter.ToString());
    }
}
