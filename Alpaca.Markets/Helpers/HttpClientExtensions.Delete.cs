using System;
using System.Collections.Generic;
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

        public static Task<TApi> DeleteSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, HttpMethod.Delete, uriBuilder.Uri, cancellationToken, throttler);

        public static Task<TApi> DeleteSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, HttpMethod.Delete, asUri(endpointUri), cancellationToken, throttler);

        public static async Task<IReadOnlyList<TApi>> DeleteObjectsListAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, HttpMethod.Delete, asUri(endpointUri), cancellationToken, throttler)
                .ConfigureAwait(false);
    }
}
