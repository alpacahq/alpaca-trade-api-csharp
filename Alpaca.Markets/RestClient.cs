using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public RestClient(
            String keyId,
            String secretKey,
            Uri restApi)
        {
            _httpClient.DefaultRequestHeaders.Add(
                "APCA-API-KEY-ID", keyId);
            _httpClient.DefaultRequestHeaders.Add(
                "APCA-API-SECRET-KEY", secretKey);
            _httpClient.BaseAddress = restApi;
        }

        private String getFormattedQueryParameters(
            IEnumerable<KeyValuePair<String, String>> queryParameters)
        {
            using (var content = new FormUrlEncodedContent(queryParameters))
            {
                return content.ReadAsStringAsync().Result;
            }
        }
    }
}
