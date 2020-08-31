using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        public static Task<Boolean> DeleteAsync(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken,
            IThrottler? throttler = null) =>
            callAndReturnSuccessCodeAsync(
                httpClient, HttpMethod.Delete, uriBuilder.Uri, cancellationToken, throttler);

        public static Task<Boolean> DeleteAsync(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null) =>
            callAndReturnSuccessCodeAsync(
                httpClient, HttpMethod.Delete, asUri(endpointUri), cancellationToken, throttler);

        public static Task<TApi> DeleteAsync<TApi, TJson>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson>(
                httpClient, HttpMethod.Delete, uriBuilder.Uri, cancellationToken, throttler);

        public static Task<TApi> DeleteAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson>(
                httpClient, HttpMethod.Delete, asUri(endpointUri), cancellationToken, throttler);
    }
}
