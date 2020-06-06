using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

        private static async Task<TApi> callAndDeserializeSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi
        {
            throttler ??= FakeThrottler.Instance;

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
            HttpMethod method,
            Uri endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
        {
            throttler ??= FakeThrottler.Instance;

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
    }
}
