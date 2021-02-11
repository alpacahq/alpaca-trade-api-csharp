using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        public static Task<TApi> PostAsync<TApi, TJson, TRequest>(
            this HttpClient httpClient,
            String endpointUri,
            TRequest request,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson, TRequest>(
                httpClient, HttpMethod.Post, asUri(endpointUri), request, cancellationToken);

        public static Task<TApi> PostAsync<TApi, TJson, TRequest>(
            this HttpClient httpClient,
            Uri endpointUri,
            TRequest request,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson, TRequest>(
                httpClient, HttpMethod.Post, endpointUri, request, cancellationToken);

        public static Task<TApi> PutAsync<TApi, TJson, TRequest>(
            this HttpClient httpClient,
            String endpointUri,
            TRequest request,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson, TRequest>(
                httpClient, HttpMethod.Put, asUri(endpointUri), request, cancellationToken);

    }
}
