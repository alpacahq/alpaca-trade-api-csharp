using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        private static readonly String _sdkVersion =
            typeof(HttpClientExtensions).Assembly.GetName().Version!.ToString();

        private static readonly Version _httpVersion =
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
            System.Net.HttpVersion.Version20;
#elif NETFRAMEWORK
            new (2, 0);
#else
            System.Net.HttpVersion.Version11;
#endif

        public static void AddAuthenticationHeaders(
            this HttpClient httpClient,
            SecurityKey securityKey)
        {
            foreach (var pair in securityKey.GetAuthenticationHeaders())
            {
                httpClient.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }
        }

        public static HttpClient Configure(
            this HttpClient httpClient,
            Uri baseAddress)
        {
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
            HttpMessageInvoker httpClient,
            HttpMethod method,
            Uri endpointUri,
            TimeSpan timeout,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            using var request = new HttpRequestMessage(method, endpointUri);
            if (timeout != Timeout.InfiniteTimeSpan)
            {
#if NET5_0_OR_GREATER
            request.Options.Set(ThrottleParameters.RequestTimeoutOptionKey, timeout);
#else
                request.Properties[ThrottleParameters.RequestTimeoutOptionKey] = timeout;
#endif
            }
            return await callAndDeserializeAsync<TApi, TJson>(
                    httpClient, request, cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<TApi> callAndDeserializeAsync<TApi, TJson>(
            HttpMessageInvoker httpClient,
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            using var request = new HttpRequestMessage(method, endpointUri);
            return await callAndDeserializeAsync<TApi, TJson>(
                httpClient, request, cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<TApi> callAndDeserializeAsync<TApi, TJson, TContent>(
            HttpMessageInvoker httpClient,
            HttpMethod method,
            Uri endpointUri,
            TContent content,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            using var request = new HttpRequestMessage(method, endpointUri) { Content = toStringContent(content) };
            return await callAndDeserializeAsync<TApi, TJson>(
                httpClient, request, cancellationToken)
                .ConfigureAwait(false);
        }

        private static async Task<TApi> callAndDeserializeAsync<TApi, TJson>(
            HttpMessageInvoker httpClient,
            HttpRequestMessage request,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            request.Version = _httpVersion;
            using var response = await httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<TApi, TJson>()
                .ConfigureAwait(false);
        }

        private static async Task<Boolean> callAndReturnSuccessCodeAsync(
            HttpMessageInvoker httpClient,
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(method, endpointUri);

            using var response = await httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return await response.IsSuccessStatusCodeAsync()
                .ConfigureAwait(false);
        }

        private static Uri asUri(String endpointUri) => new (endpointUri, UriKind.RelativeOrAbsolute);

        private static StringContent toStringContent<T>(T value)
        {
            var serializer = new JsonSerializer();
            using var stringWriter = new StringWriter();

            serializer.Serialize(stringWriter, value);
            return new StringContent(stringWriter.ToString());
        }
    }
}
