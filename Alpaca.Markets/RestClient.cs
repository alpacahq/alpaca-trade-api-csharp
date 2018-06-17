using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Alpaca.Markets.Helpers;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed class RestClient
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

        public async Task<IAccount> GetAccountAsync()
        {
            using (var stream = await _httpClient.GetStreamAsync("v1/account"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonAccount>(reader);
            }
        }

        public async Task<IEnumerable<IAsset>> GetAssetsAsync(
            AssetStatus? assetStatus = null,
            AssetClass? assetClass = null)
        {
            var queryParameters = new Dictionary<String, String>();
            if (assetStatus.HasValue)
            {
                queryParameters.Add("status", assetStatus.Value.ToEnumString());
            }
            if (assetClass.HasValue)
            {
                queryParameters.Add("asset_class", assetClass.Value.ToEnumString());
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/assets",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonAsset>>(reader);
            }
        }

        public async Task<IAsset> GetAssetAsync(
            String symbol)
        {
            using (var stream = await _httpClient.GetStreamAsync($"v1/assets/{symbol}"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonAsset>(reader);
            }
        }

        public async Task<IEnumerable<IPosition>> GetPositionsAsync()
        {
            using (var stream = await _httpClient.GetStreamAsync("v1/positions"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonPosition>>(reader);
            }
        }

        public async Task<IPosition> GetPositionAsync(
            String symbol)
        {
            using (var stream = await _httpClient.GetStreamAsync($"v1/positions/{symbol}"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonPosition>(reader);
            }
        }

        public async Task<IClock> GetClockAsync()
        {
            using (var stream = await _httpClient.GetStreamAsync("v1/clock"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonClock>(reader);
            }
        }

        public async Task<IEnumerable<ICalendar>> GetCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null)
        {
            var queryParameters = new Dictionary<String, String>();
            if (startDateInclusive.HasValue)
            {
                queryParameters.Add("start", startDateInclusive.Value
                    .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }
            if (endDateInclusive.HasValue)
            {
                queryParameters.Add("end", endDateInclusive.Value
                    .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/calendar",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonCalendar>>(reader);
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
