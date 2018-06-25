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
        public Task<IAccount> GetAccountAsync()
        {
            return getSingleObjectAsync<IAccount, JsonAccount>(_alpacaHttpClient, "v1/account");
        }

        public Task<IEnumerable<IAsset>> GetAssetsAsync(
            AssetStatus? assetStatus = null,
            AssetClass? assetClass = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/assets",
                Query = new QueryBuilder()
                    .AddParameter("status", assetStatus)
                    .AddParameter("asset_class", assetClass)
            };

            return getObjectsListAsync<IAsset, JsonAsset>(_alpacaHttpClient, builder);
        }

        public Task<IAsset> GetAssetAsync(
            String symbol)
        {
            return getSingleObjectAsync<IAsset, JsonAsset>(_alpacaHttpClient, $"v1/assets/{symbol}");
        }

        public Task<IEnumerable<IOrder>> GetOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            DateTime? untilDateTime = null,
            Int64? limitOrderNumber = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/orders",
                Query = new QueryBuilder()
                    .AddParameter("status", orderStatusFilter)
                    .AddParameter("until", untilDateTime)
                    .AddParameter("limit", limitOrderNumber)
            };

            return getObjectsListAsync<IOrder, JsonOrder>(_alpacaHttpClient, builder);
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
                using (var response = await _alpacaHttpClient.PostAsync("v1/orders", content))
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

        public Task<IOrder> GetOrderAsync(
            String clientOrderId)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/orders:by_client_order_id",
                Query = new QueryBuilder()
                    .AddParameter("client_order_id", clientOrderId)
            };

            return getSingleObjectAsync<IOrder, JsonOrder>(_alpacaHttpClient, builder);
        }

        public Task<IOrder> GetOrderAsync(
            Guid orderId)
        {
            return getSingleObjectAsync<IOrder, JsonOrder>(_alpacaHttpClient, $"v1/orders/{orderId:D}");
        }

        public async Task<Boolean> DeleteOrderAsync(
            Guid orderId)
        {
            using (var response = await _alpacaHttpClient.DeleteAsync($"v1/orders/{orderId:D}"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        public Task<IEnumerable<IPosition>> GetPositionsAsync()
        {
            return getObjectsListAsync<IPosition, JsonPosition>(_alpacaHttpClient, "v1/positions");
        }

        public Task<IPosition> GetPositionAsync(
            String symbol)
        {
            return getSingleObjectAsync<IPosition, JsonPosition>(_alpacaHttpClient, $"v1/positions/{symbol}");
        }

        public Task<IClock> GetClockAsync()
        {
            return getSingleObjectAsync<IClock, JsonClock>(_alpacaHttpClient, "v1/clock");
        }

        public Task<IEnumerable<ICalendar>> GetCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/calendar",
                Query = new QueryBuilder()
                    .AddParameter("start", startDateInclusive, "yyyy-MM-dd")
                    .AddParameter("end", endDateInclusive, "yyyy-MM-dd")
            };

            return getObjectsListAsync<ICalendar, JsonCalendar>(_alpacaHttpClient, builder);
        }
    }
}