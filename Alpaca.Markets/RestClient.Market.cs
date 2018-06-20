using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Alpaca.Markets.Helpers;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        public async Task<IEnumerable<IAssetBars>> GetBarsAsync(
            IEnumerable<String> symbol,
            BarDuration barDuration,
            DateTime? startTimeInclusive = null,
            DateTime? endTimeInclusive = null)
        {
            var queryParameters = new Dictionary<String, String>
            {
                {"symbols", String.Join(",", symbol)},
                {"timeframe ", barDuration.ToEnumString()}
            };

            if (startTimeInclusive.HasValue)
            {
                queryParameters.Add("start_dt", startTimeInclusive.Value
                    .ToString("O", CultureInfo.InvariantCulture));
            }

            if (endTimeInclusive.HasValue)
            {
                queryParameters.Add("end_dt", endTimeInclusive.Value
                    .ToString("O", CultureInfo.InvariantCulture));
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/bars",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonAssetBars>>(reader);
            }
        }

        public async Task<IAssetBars> GetBarsAsync(
            String symbol,
            BarDuration barDuration,
            DateTime? startTimeInclusive = null,
            DateTime? endTimeInclusive = null)
        {
            var queryParameters = new Dictionary<String, String>
            {
                {"timeframe ", barDuration.ToEnumString()}
            };

            if (startTimeInclusive.HasValue)
            {
                queryParameters.Add("start_dt", startTimeInclusive.Value
                    .ToString("O", CultureInfo.InvariantCulture));
            }

            if (endTimeInclusive.HasValue)
            {
                queryParameters.Add("end_dt", endTimeInclusive.Value
                    .ToString("O", CultureInfo.InvariantCulture));
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"v1/assets/{symbol}/bars",
                Query = getFormattedQueryParameters(queryParameters)
            };

            var res = await _httpClient.GetStringAsync(builder.Uri);

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonAssetBars>(reader);
            }
        }

        public async Task<IEnumerable<IQuote>> GetQuotesAsync(
            IEnumerable<String> symbol)
        {
            var queryParameters = new Dictionary<String, String>
            {
                {"symbols", String.Join(",", symbol)}
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

        public Task<IQuote> GetQuoteAsync(
            String symbol)
        {
            return getSingleObjectAsync<IQuote, JsonQuote>($"v1/assets/{symbol}/quote");
        }

        public async Task<IEnumerable<IFundamental>> GetFundamentalsAsync(
            IEnumerable<String> symbol)
        {
            var queryParameters = new Dictionary<String, String>
            {
                {"symbols", String.Join(",", symbol)}
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

        public Task<IFundamental> GetFundamentalAsync(
            String symbol)
        {
            return getSingleObjectAsync<IFundamental, JsonFundamental>($"v1/assets/{symbol}/fundamental");
        }
    }
}