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

        public static async Task<TApi> PatchAsync<TApi, TJson>(
            this HttpClient httpClient,
            String endpointUri,
            HttpContent content,
            IThrottler throttler,
            CancellationToken cancellationToken)
            where TJson : TApi
        {
            await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var request = new HttpRequestMessage(_httpMethodPatch, asUri(endpointUri))
            {
                Content = content
            };

            using var response = await httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<TApi, TJson>()
                .ConfigureAwait(false);
        }
    }
}
