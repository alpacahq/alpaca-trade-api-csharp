using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        public static void AddAuthenticationHeaders(
            this HttpClient httpClient,
            SecurityKey securityKey)
        {
            foreach (var pair in securityKey.GetAuthenticationHeaders())
            {
                httpClient.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }
        }

        // ReSharper disable once StringLiteralTypo
        [Conditional("NETFRAMEWORK")]
        public static void SetSecurityProtocol(
            // ReSharper disable once UnusedParameter.Global
            this HttpClient httpClient) =>
            // ReSharper disable once StringLiteralTypo
            AppContext.SetSwitch("DontEnableSystemDefaultTlsVersions", false);

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
#if NET5_0_OR_GREATER || NETSTANDARD2_1
            request.Version = System.Net.HttpVersion.Version20;
#endif
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
