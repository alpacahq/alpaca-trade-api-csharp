﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        /// <summary>
        /// Gets account information from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Read-only account information.</returns>
        public Task<IAccount> GetAccountAsync()
        {
            return getSingleObjectAsync<IAccount, JsonAccount>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "v1/account");
        }

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="assetStatus">Asset status for filtering.</param>
        /// <param name="assetClass">Asset class for filtering.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IEnumerable<IAsset>> ListAssetsAsync(
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

            return getObjectsListAsync<IAsset, JsonAsset>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
        }

        /// <summary>
        /// Get single asset information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for searching.</param>
        /// <returns>Read-only asset information.</returns>
        public Task<IAsset> GetAssetAsync(
            String symbol)
        {
            return getSingleObjectAsync<IAsset, JsonAsset>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"v1/assets/{symbol}");
        }

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderStatusFilter">Order status for filtering.</param>
        /// <param name="untilDateTime">Returns only orders until specified date.</param>
        /// <param name="limitOrderNumber">Maximal number of orders in response.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IEnumerable<IOrder>> ListOrdersAsync(
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

            return getObjectsListAsync<IOrder, JsonOrder>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
        }

        /// <summary>
        /// Creates new order for execution using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="side">Order size (buy or sell).</param>
        /// <param name="type">Order type.</param>
        /// <param name="duration">Order duration.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="clientOrderId">Client order ID.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
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

            _alpacaRestApiThrottler.WaitToProceed();

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

        /// <summary>
        /// Get single order information by client order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="clientOrderId">Client order ID for searching.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            String clientOrderId)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = "v1/orders:by_client_order_id",
                Query = new QueryBuilder()
                    .AddParameter("client_order_id", clientOrderId)
            };

            return getSingleObjectAsync<IOrder, JsonOrder>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
        }

        /// <summary>
        /// Get single order information by server order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for searching.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            Guid orderId)
        {
            return getSingleObjectAsync<IOrder, JsonOrder>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"v1/orders/{orderId:D}");
        }

        /// <summary>
        /// Deletes/cancel order on server by server order ID using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for cancelling.</param>
        /// <returns><c>True</c> if order deleted/cancelled successfully.</returns>
        public async Task<Boolean> DeleteOrderAsync(
            Guid orderId)
        {
            _alpacaRestApiThrottler.WaitToProceed();

            using (var response = await _alpacaHttpClient.DeleteAsync($"v1/orders/{orderId:D}"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Read-only list of position information objects.</returns>
        public Task<IEnumerable<IPosition>> ListPositionsAsync()
        {
            return getObjectsListAsync<IPosition, JsonPosition>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "v1/positions");
        }

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <returns>Read-only position information object.</returns>
        public Task<IPosition> GetPositionAsync(
            String symbol)
        {
            return getSingleObjectAsync<IPosition, JsonPosition>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"v1/positions/{symbol}");
        }

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Read-only clock information object.</returns>
        public Task<IClock> GetClockAsync()
        {
            return getSingleObjectAsync<IClock, JsonClock>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "v1/clock");
        }

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDateInclusive">Start time for filtering (inclusive).</param>
        /// <param name="endDateInclusive">End time for filtering (inclusive).</param>
        /// <returns>Read-only list of trading date information object.</returns>
        public Task<IEnumerable<ICalendar>> ListCalendarAsync(
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

            return getObjectsListAsync<ICalendar, JsonCalendar>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
        }

        /// <summary>
        /// Gets lookup table of historical daily bars lists for all assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbols">>Asset names for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="areTimesInclusive">
        /// If <c>true</c> - both <paramref name="startTime"/> and <paramref name="endTime"/> parameters are treated as inclusive.
        /// </param>
        /// <param name="startTime">Start time for filtering.</param>
        /// <param name="endTime">End time for filtering.</param>
        /// <param name="limit">Maximal number of daily bars in data response.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public async Task<ILookup<string, IEnumerable<IAgg>>> GetBarSetAsync(
            IEnumerable<String> symbols,
            TimeFrame timeFrame,
            Int32? limit = 100,
            Boolean areTimesInclusive = true,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            var builder = new UriBuilder(_alpacaDataClient.BaseAddress)
            {
                Path = "v1/bars/" + timeFrame.ToEnumString(),
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", symbols))
                    .AddParameter((areTimesInclusive ? "start" : "after"), startTime)
                    .AddParameter((areTimesInclusive ? "end" : "until"), endTime)
                    .AddParameter("limit", limit)
            };

            var response = await getSingleObjectAsync
                <IReadOnlyDictionary<String, List<JsonBarAgg>>,
                    Dictionary<String, List<JsonBarAgg>>>(
                _alpacaHttpClient, FakeThrottler.Instance, builder);

            return response.ToLookup(kvp => kvp.Key, kvp => kvp.Value.AsEnumerable<IAgg>());
        }
    }
}