using System;
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
        public Task<IAccount> GetAccountAsync() =>
            getSingleObjectAsync<IAccount, JsonAccount>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "account");

        /// <summary>
        /// Gets account configuration settings from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Mutable version of account configuration object.</returns>
        public Task<IAccountConfiguration> GetAccountConfigurationAsync()
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "account/configurations",
            };

            return getSingleObjectAsync<IAccountConfiguration, JsonAccountConfiguration>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
        }

        /// <summary>
        /// Updates account configuration settings using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="accountConfiguration">New account configuration object for updating.</param>
        /// <returns>Mutable version of updated account configuration object.</returns>
        public async Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration)
        {
            await _alpacaRestApiThrottler.WaitToProceed().ConfigureAwait(false);

            using (var request = new HttpRequestMessage(_httpMethodPatch,
                new Uri("account/configurations", UriKind.RelativeOrAbsolute)))
            {
                var serializer = new JsonSerializer();
                using (var stringWriter = new StringWriter())
                {
                    serializer.Serialize(stringWriter, accountConfiguration);
                    request.Content = new StringContent(stringWriter.ToString());
                }

                using (var response = await _alpacaHttpClient.SendAsync(request)
                    .ConfigureAwait(false))
                {
                    return await deserializeAsync<IAccountConfiguration, JsonAccountConfiguration>(response)
                        .ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="activityTypes">The activity type you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IEnumerable<IAsset>> ListAccountActivitiesAsync(
            IEnumerable<AccountActivityType> activityTypes = null,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String pageToken = null)
        {
            if (date.HasValue && (until.HasValue || after.HasValue))
            {
                throw new ArgumentException("You unable to specify 'date' and 'until'/'after' arguments in same call.");
            }

            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "account/activities",
                Query = new QueryBuilder()
                    .AddParameter("activity_types", string.Join(",", activityTypes))
                    .AddParameter("date", date)
                    .AddParameter("until", until)
                    .AddParameter("after", after)
                    .AddParameter("direction", direction)
                    .AddParameter("pageSize", pageSize)
                    .AddParameter("pageToken", pageToken)
            };

            return getObjectsListAsync<IAsset, JsonAsset>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder);
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
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "assets",
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
            String symbol) =>
            getSingleObjectAsync<IAsset, JsonAsset>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"assets/{symbol}");

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderStatusFilter">Order status for filtering.</param>
        /// <param name="orderListSorting">The chronological order of response based on the submission time.</param>
        /// <param name="untilDateTimeExclusive">Returns only orders until specified timestamp (exclusive).</param>
        /// <param name="afterDateTimeExclusive">Returns only orders after specified timestamp (exclusive).</param>
        /// <param name="limitOrderNumber">Maximal number of orders in response.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IEnumerable<IOrder>> ListOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            SortDirection? orderListSorting = null,
            DateTime? untilDateTimeExclusive = null,
            DateTime? afterDateTimeExclusive = null,
            Int64? limitOrderNumber = null)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "orders",
                Query = new QueryBuilder()
                    .AddParameter("status", orderStatusFilter)
                    .AddParameter("direction", orderListSorting)
                    .AddParameter("until", untilDateTimeExclusive, "O")
                    .AddParameter("after", afterDateTimeExclusive, "O")
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
        /// <param name="side">Order side (buy or sell).</param>
        /// <param name="type">Order type.</param>
        /// <param name="duration">Order duration.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="clientOrderId">Client order ID.</param>
        /// <param name="extendedHours">Whether or not this order should be allowed to execute during extended hours trading.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        public async Task<IOrder> PostOrderAsync(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String clientOrderId = null,
            Boolean? extendedHours = null)
        {
            if (!string.IsNullOrEmpty(clientOrderId) &&
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
                ClientOrderId = clientOrderId,
                ExtendedHours = extendedHours
            };

            await _alpacaRestApiThrottler.WaitToProceed();

            var serializer = new JsonSerializer();
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, newOrder);

                using (var content = new StringContent(stringWriter.ToString()))
                using (var response = await _alpacaHttpClient.PostAsync("orders", content))
                {
                    return await deserializeAsync<IOrder, JsonOrder>(response);
                }
            }
        }
        
        /// <summary>
        /// Updates existing order using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server side order identifier.</param>
        /// <param name="quantity">Updated order quantity or <c>null</c> if quantity is not changed.</param>
        /// <param name="duration">Updated order duration or <c>null</c> if duration is not changed.</param>
        /// <param name="limitPrice">Updated order limit price or <c>null</c> if limit price is not changed.</param>
        /// <param name="stopPrice">Updated order stop price or <c>null</c> if stop price is not changed.</param>
        /// <param name="clientOrderId">Updated client order ID or <c>null</c> if client order ID is not changed.</param>
        /// <returns>Read-only order information object for updated order.</returns>
        public async Task<IOrder> PatchOrderAsync(
            Guid orderId,
            Int64? quantity = null,
            TimeInForce? duration = null,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String clientOrderId = null)
        {
            if (!string.IsNullOrEmpty(clientOrderId) &&
                clientOrderId.Length > 48)
            {
                clientOrderId = clientOrderId.Substring(0, 48);
            }

            var changeOrder = new JsonChangeOrder
            {
                Quantity = quantity,
                TimeInForce = duration,
                LimitPrice = limitPrice,
                StopPrice = stopPrice,
                ClientOrderId = clientOrderId,
            };

            await _alpacaRestApiThrottler.WaitToProceed().ConfigureAwait(false);

            using (var request = new HttpRequestMessage(_httpMethodPatch,
                new Uri($"orders/{orderId:D}", UriKind.RelativeOrAbsolute)))
            {
                using (var stringWriter = new StringWriter())
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(stringWriter, changeOrder);
                    request.Content = new StringContent(stringWriter.ToString());
                }

                using (var response = await _alpacaHttpClient.SendAsync(request)
                    .ConfigureAwait(false))
                {
                    return await deserializeAsync<IOrder, JsonOrder>(response)
                        .ConfigureAwait(false);
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
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "orders:by_client_order_id",
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
            Guid orderId) =>
            getSingleObjectAsync<IOrder, JsonOrder>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"orders/{orderId:D}");

        /// <summary>
        /// Deletes/cancel order on server by server order ID using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for cancelling.</param>
        /// <returns><c>True</c> if order cancellation was accepted.</returns>
        public async Task<Boolean> DeleteOrderAsync(
            Guid orderId)
        {
            await _alpacaRestApiThrottler.WaitToProceed();

            using (var response = await _alpacaHttpClient.DeleteAsync($"orders/{orderId:D}"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Deletes/cancel all open orders using Alpaca REST API endpoint.
        /// </summary>
        /// <returns><c>True</c> if order deleted/cancelled successfully.</returns>
        public async Task<Boolean> DeleteAllOrdersAsync()
        {
            await _alpacaRestApiThrottler.WaitToProceed();

            using (var response = await _alpacaHttpClient.DeleteAsync($"orders"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Read-only list of position information objects.</returns>
        public Task<IEnumerable<IPosition>> ListPositionsAsync() =>
            getObjectsListAsync<IPosition, JsonPosition>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "positions");

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <returns>Read-only position information object.</returns>
        public Task<IPosition> GetPositionAsync(
            String symbol) =>
            getSingleObjectAsync<IPosition, JsonPosition>(
                _alpacaHttpClient, _alpacaRestApiThrottler, $"positions/{symbol}");

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <returns><c>True</c> if positions deleted/liquidated successfully.</returns>
        public async Task<Boolean> DeleteAllPositions()
        {
            await _alpacaRestApiThrottler.WaitToProceed();

            using (var response = await _alpacaHttpClient.DeleteAsync($"positions"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        /// <returns><c>True</c> if position liquidation was accepted.</returns>
        public async Task<Boolean> DeletePositionAsync(
            String symbol)
        {
            await _alpacaRestApiThrottler.WaitToProceed();

            using (var response = await _alpacaHttpClient.DeleteAsync($"positions/{symbol}"))
            {
                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <returns>Read-only clock information object.</returns>
        public Task<IClock> GetClockAsync() =>
            getSingleObjectAsync<IClock, JsonClock>(
                _alpacaHttpClient, _alpacaRestApiThrottler, "clock");

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
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "calendar",
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
        /// If <c>true</c> - both <paramref name="timeFrom"/> and <paramref name="timeInto"/> parameters are treated as inclusive.
        /// </param>
        /// <param name="timeFrom">Start time for filtering.</param>
        /// <param name="timeInto">End time for filtering.</param>
        /// <param name="limit">Maximal number of daily bars in data response.</param>
        /// <returns>Read-only list of daily bars for specified asset.</returns>
        public async Task<IReadOnlyDictionary<String, IEnumerable<IAgg>>> GetBarSetAsync(
            IEnumerable<String> symbols,
            TimeFrame timeFrame,
            Int32? limit = 100,
            Boolean areTimesInclusive = true,
            DateTime? timeFrom = null,
            DateTime? timeInto = null)
        {
            var builder = new UriBuilder(_alpacaDataClient.BaseAddress)
            {
                Path = _alpacaDataClient.BaseAddress.AbsolutePath + $"bars/{timeFrame.ToEnumString()}",
                Query = new QueryBuilder()
                    .AddParameter("symbols", string.Join(",", symbols))
                    .AddParameter((areTimesInclusive ? "start" : "after"), timeFrom, "O")
                    .AddParameter((areTimesInclusive ? "end" : "until"), timeInto, "O")
                    .AddParameter("limit", limit)
            };

            var response = await getSingleObjectAsync
                <IReadOnlyDictionary<String, List<JsonBarAgg>>,
                    Dictionary<String, List<JsonBarAgg>>>(
                _alpacaHttpClient, FakeThrottler.Instance, builder);

            return response.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsEnumerable<IAgg>());
        }
    }
}
