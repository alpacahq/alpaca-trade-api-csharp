using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal static partial class HttpClientExtensions
    {
        private static readonly HttpMethod _httpMethodPatch = 
#if NETSTANDARD2_1
            HttpMethod.Patch;
#else
            new HttpMethod("PATCH");
#endif

        public static Task<TApi> PatchAsync<TApi, TJson, TRequest>(
            this HttpClient httpClient,
            String endpointUri,
            TRequest request,
            IThrottler throttler,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeAsync<TApi, TJson, TRequest>(
                httpClient, _httpMethodPatch, asUri(endpointUri), request, cancellationToken, throttler);
    }
}
