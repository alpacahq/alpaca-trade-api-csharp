using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    internal static class HttpClientExtensions
    {
        public static Task<TApi> GetSingleObjectAsync<TApi, TJson>(
            this HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, uriBuilder.Uri, cancellationToken);

        private static async Task<TApi> callAndDeserializeSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            Uri endpointUri,
            CancellationToken cancellationToken,
            HttpMethod? method = null)
            where TJson : TApi
        {
            var exceptions = new Queue<Exception>();

            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);
                try
                {
                    using var request = new HttpRequestMessage(method ?? HttpMethod.Get, endpointUri);
                    using var response = await httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false);

                    // Check response for server and caller specified waits and retries
                    if (!throttler.CheckHttpResponse(response))
                    {
                        continue;
                    }

                    return await deserializeAsync<TApi, TJson>(response).ConfigureAwait(false);
                }
                catch (HttpRequestException ex)
                {
                    exceptions.Enqueue(ex);
                    break;
                }
            }

            throw new AggregateException(exceptions);
        }

        private static async Task<TApi> deserializeAsync<TApi, TJson>(
            HttpResponseMessage response)
            where TJson : TApi
        {
#if NETSTANDARD2_1
            await using var stream = await response.Content.ReadAsStreamAsync()
#else
            using var stream = await response.Content.ReadAsStreamAsync()
#endif
                .ConfigureAwait(false);
            using var reader = new JsonTextReader(new StreamReader(stream));

            var serializer = new JsonSerializer();
            if (response.IsSuccessStatusCode)
            {
                return serializer.Deserialize<TJson>(reader);
            }

            try
            {
                throw new RestClientErrorException(
                    serializer.Deserialize<JsonError>(reader) ?? new JsonError());
            }
            catch (Exception exception)
            {
                throw new RestClientErrorException(response, exception);
            }
        }

        private static async Task<IReadOnlyList<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                httpClient, throttler, uriBuilder.Uri, cancellationToken)
                .ConfigureAwait(false);

        private static async Task<IReadOnlyList<TApi>> deleteObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, throttler, uriBuilder.Uri, cancellationToken, HttpMethod.Delete)
                .ConfigureAwait(false);    }
}
