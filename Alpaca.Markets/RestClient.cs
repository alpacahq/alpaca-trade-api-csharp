using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        private readonly HttpClient _alpacaHttpClient = new HttpClient();

        private readonly HttpClient _polygonHttpClient = new HttpClient();

        public RestClient(
            String keyId,
            String secretKey,
            Uri restApi)
        {
            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-KEY-ID", keyId);
            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-SECRET-KEY", secretKey);
            _alpacaHttpClient.BaseAddress = restApi;

            _polygonHttpClient.DefaultRequestHeaders.Add(
                "api-key", keyId);
            _polygonHttpClient.BaseAddress = 
                new Uri("https://api.polygon.io");
        }

        private async Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            String endpointUri)
            where TJson : TApi
        {
            using (var stream = await httpClient.GetStreamAsync(endpointUri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<TJson>(reader);
            }
        }

        private Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            UriBuilder uriBuilder)
            where TJson : TApi
        {
            return getSingleObjectAsync<TApi, TJson>(httpClient, uriBuilder.ToString());
        }

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            String endpointUri)
            where TJson : TApi
        {
            return (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, endpointUri);
        }

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            UriBuilder uriBuilder)
            where TJson : TApi
        {
            return (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, uriBuilder);
        }
    }
}
