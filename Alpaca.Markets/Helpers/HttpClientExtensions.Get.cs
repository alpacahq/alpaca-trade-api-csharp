using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        public static Task<TApi> GetSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, HttpMethod.Get, uriBuilder.Uri, cancellationToken, throttler);

        public static Task<TApi> GetSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, HttpMethod.Get, asUri(endpointUri), cancellationToken, throttler);

        public static async Task<IReadOnlyList<TApi>> GetObjectsListAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, HttpMethod.Get, asUri(endpointUri), cancellationToken, throttler)
                .ConfigureAwait(false);

        public static async Task<IReadOnlyList<TApi>> GetObjectsListAsync<TApi, TJson>(
            this HttpClient httpClient,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken,
            IThrottler? throttler = null)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, HttpMethod.Get, uriBuilder.Uri, cancellationToken, throttler)
                .ConfigureAwait(false);
    }
}
