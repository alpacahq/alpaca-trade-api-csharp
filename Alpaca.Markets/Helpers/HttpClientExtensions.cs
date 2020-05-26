using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal static class HttpClientExtensions
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

        public static Task<TApi> GetSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, HttpMethod.Get, uriBuilder.Uri, cancellationToken);

        public static Task<TApi> GetSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            String endpointUri,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, HttpMethod.Get, asUri(endpointUri), cancellationToken);

        public static async Task<IReadOnlyList<TApi>> GetObjectsListAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, throttler, HttpMethod.Get, uriBuilder.Uri, cancellationToken)

                .ConfigureAwait(false);

        public static Task<Boolean> DeleteAsync(
            this HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken) =>
            callAndReturnSuccessCodeAsync(
                httpClient, throttler, HttpMethod.Delete, uriBuilder.Uri, cancellationToken);

        public static Task<Boolean> DeleteAsync(
            this HttpClient httpClient,
            IThrottler throttler,
            String endpointUri,
            CancellationToken cancellationToken) =>
            callAndReturnSuccessCodeAsync(
                httpClient, throttler, HttpMethod.Delete, asUri(endpointUri), cancellationToken);

        public static Task<TApi> DeleteSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, HttpMethod.Delete, uriBuilder.Uri, cancellationToken);

        public static Task<TApi> DeleteSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            String endpointUri,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, HttpMethod.Delete, asUri(endpointUri), cancellationToken);

        public static async Task<IReadOnlyList<TApi>> DeleteObjectsListAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            String endpointUri,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, throttler, HttpMethod.Delete, asUri(endpointUri), cancellationToken)
                .ConfigureAwait(false);

        private static async Task<TApi> callAndDeserializeSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);

                using var request = new HttpRequestMessage(method, endpointUri);
                using var response = await httpClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

                // Check response for server and caller specified waits and retries
                if (throttler.CheckHttpResponse(response))
                {
                    return await response.DeserializeAsync<TApi, TJson>()
                        .ConfigureAwait(false);
                }
            }

            throw new RestClientErrorException(
                $"Unable to successfully call REST API endpoint `{endpointUri}` after {throttler.MaxRetryAttempts} attempts.");
        }

        private static async Task<Boolean> callAndReturnSuccessCodeAsync(
            HttpClient httpClient,
            IThrottler throttler,
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken)
        {
            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);

                using var request = new HttpRequestMessage(method, endpointUri);
                using var response = await httpClient
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                    .ConfigureAwait(false);

                // Check response for server and caller specified waits and retries
                if (throttler.CheckHttpResponse(response))
                {
                    return await response.IsSuccessStatusCodeAsync()
                        .ConfigureAwait(false);
                }
            }

            throw new RestClientErrorException(
                $"Unable to successfully call REST API endpoint `{endpointUri}` after {throttler.MaxRetryAttempts} attempts.");
        }

        private static Uri asUri(String endpointUri) => new Uri(endpointUri, UriKind.RelativeOrAbsolute);

        [Conditional("NET45")]
        public static void SetSecurityProtocol(
            // ReSharper disable once UnusedParameter.Global
            this HttpClient httpClient)
        {
#if NET45
            System.Net.ServicePointManager.SecurityProtocol =
#pragma warning disable CA5364 // Do Not Use Deprecated Security Protocols
                System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
#pragma warning restore CA5364 // Do Not Use Deprecated Security Protocols
#endif
        }
    }
}
