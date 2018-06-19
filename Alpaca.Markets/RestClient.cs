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

        public async Task<IEnumerable<IQuote>> GetQuotesAsync(
            IEnumerable<String> symbol)
        {
            var queryParameters = new Dictionary<String, String>
            {
                { "symbols", String.Join(",", symbol) }
            };

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/quotes",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonQuote>>(reader);
            }
        }

        public async Task<IQuote> GetQuoteAsync(
            String symbol)
        {
            using (var stream = await _httpClient.GetStreamAsync($"v1/assets/{symbol}/quote"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonQuote>(reader);
            }
        }

        public async Task<IEnumerable<IFundamental>> GetFundamentalsAsync(
            IEnumerable<String> symbol)
        {
            var queryParameters = new Dictionary<String, String>
            {
                { "symbols", String.Join(",", symbol) }
            };

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/fundamentals",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonFundamental>>(reader);
            }
        }

        public async Task<IFundamental> GetFundamentalAsync(
            String symbol)
        {
            var res = await _httpClient.GetStringAsync($"v1/assets/{symbol}/fundamental");

            using (var stream = await _httpClient.GetStreamAsync($"v1/assets/{symbol}/fundamental"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonFundamental>(reader);
            }
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
