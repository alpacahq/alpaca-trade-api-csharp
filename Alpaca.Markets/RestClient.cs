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

        public async Task<IEnumerable<IOrder>> GetOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            DateTime? untilDateTime = null,
            Int64? limitOrderNumber = null)
        {
            var queryParameters = new Dictionary<String, String>();
            if (orderStatusFilter.HasValue)
            {
                queryParameters.Add("status", orderStatusFilter.Value.ToEnumString());
            }
            if (untilDateTime.HasValue)
            {
                queryParameters.Add("until", untilDateTime.Value
                    .ToString("O", CultureInfo.InvariantCulture));
            }
            if (limitOrderNumber.HasValue)
            {
                queryParameters.Add("limit", limitOrderNumber.Value
                    .ToString("D", CultureInfo.InvariantCulture));
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/orders",
                Query = getFormattedQueryParameters(queryParameters)
            };

            var res = await _httpClient.GetStringAsync(builder.Uri);

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<List<JsonOrder>>(reader);
            }
        }

        public async Task<IOrder> PostOrderAsync(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String clientOrderId = null)
        {
            if (!String.IsNullOrEmpty(clientOrderId) &&
                clientOrderId.Length > 48)
            {
                clientOrderId = clientOrderId.Substring(0, 48);
            }

            var newOrder = new JsonNewOrder
            {
                Symbol = symbol,
                Quantity = quantity,
                OrderSide = side,
                OrderType = type,
                TimeInForce = duration,
                LimitPrice = limitPrice,
                StopPrice = stopPrice,
                ClientOrderId = clientOrderId
            };

            var serializer = new JsonSerializer();
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, newOrder);

                using (var content = new StringContent(stringWriter.ToString()))
                using (var response = await _httpClient.PostAsync("v1/orders", content))
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var textReader = new StreamReader(stream))
                using (var reader = new JsonTextReader(textReader))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return serializer.Deserialize<JsonOrder>(reader);
                    }

                    var error = serializer.Deserialize<JsonError>(reader);
                    throw new RestClientErrorException(error);
                }
            }
        }

        public async Task<IOrder> GetOrderAsync(
            String clientOrderId)
        {
            var queryParameters = new Dictionary<String, String>
            {
                { "client_order_id", clientOrderId.Trim() }
            };

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v1/orders:by_client_order_id",
                Query = getFormattedQueryParameters(queryParameters)
            };

            using (var stream = await _httpClient.GetStreamAsync(builder.Uri))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonOrder>(reader);
            }
        }

        public async Task<IOrder> GetOrderAsync(
            Guid orderId)
        {
            using (var stream = await _httpClient.GetStreamAsync($"v1/orders/{orderId:D}"))
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JsonOrder>(reader);
            }
        }

        public async Task<Boolean> DeleteOrderAsync(
            Guid orderId)
        {
            using (var response = await _httpClient.DeleteAsync($"v1/orders/{orderId:D}"))
            {
                return response.IsSuccessStatusCode;
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
