using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <summary>
        /// Gets account information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only account information.</returns>
        public Task<IAccount> GetAccountAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "account",
            };

            return _httpClient.GetSingleObjectAsync<IAccount, JsonAccount>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets account configuration settings from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of account configuration object.</returns>
        public Task<IAccountConfiguration> GetAccountConfigurationAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "account/configurations",
            };

            return _httpClient.GetSingleObjectAsync<IAccountConfiguration, JsonAccountConfiguration>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Updates account configuration settings using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="accountConfiguration">New account configuration object for updating.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of updated account configuration object.</returns>
        public async Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration,
            CancellationToken cancellationToken = default)
        {
            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var request = new HttpRequestMessage(_httpMethodPatch,
                new Uri("account/configurations", UriKind.RelativeOrAbsolute))
            {
                Content = toStringContent(accountConfiguration)
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IAccountConfiguration, JsonAccountConfiguration>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint by specific activity.
        /// </summary>
        /// <param name="activityType">The activity type you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivityType activityType,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default)
        {
            if (date.HasValue && (until.HasValue || after.HasValue))
            {
                throw new ArgumentException("You unable to specify 'date' and 'until'/'after' arguments in same call.");
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"account/activities/{activityType.ToEnumString()}",
                Query = new QueryBuilder()
                    .AddParameter("date", date, "yyyy-MM-dd")
                    .AddParameter("until", until, "O")
                    .AddParameter("after", after, "O")
                    .AddParameter("direction", direction)
                    .AddParameter("pageSize", pageSize)
                    .AddParameter("pageToken", pageToken)
            };

            return _httpClient.GetObjectsListAsync<IAccountActivity, JsonAccountActivity>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="activityTypes">The list of activity types you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            IEnumerable<AccountActivityType>? activityTypes = null,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default)
        {
            if (date.HasValue && (until.HasValue || after.HasValue))
            {
                throw new ArgumentException("You unable to specify 'date' and 'until'/'after' arguments in same call.");
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "account/activities",
                Query = new QueryBuilder()
                    .AddParameter("activity_types", activityTypes)
                    .AddParameter("date", date, "yyyy-MM-dd")
                    .AddParameter("until", until, "O")
                    .AddParameter("after", after, "O")
                    .AddParameter("direction", direction)
                    .AddParameter("pageSize", pageSize)
                    .AddParameter("pageToken", pageToken)
            };

            return _httpClient.GetObjectsListAsync<IAccountActivity, JsonAccountActivity>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="assetStatus">Asset status for filtering.</param>
        /// <param name="assetClass">Asset class for filtering.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetStatus? assetStatus = null,
            AssetClass? assetClass = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "assets",
                Query = new QueryBuilder()
                    .AddParameter("status", assetStatus)
                    .AddParameter("asset_class", assetClass)
            };

            return _httpClient.GetObjectsListAsync<IAsset, JsonAsset>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Get single asset information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only asset information.</returns>
        public Task<IAsset> GetAssetAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"assets/{symbol}",
            };

            return _httpClient.GetSingleObjectAsync<IAsset, JsonAsset>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderStatusFilter">Order status for filtering.</param>
        /// <param name="orderListSorting">The chronological order of response based on the submission time.</param>
        /// <param name="untilDateTimeExclusive">Returns only orders until specified timestamp (exclusive).</param>
        /// <param name="afterDateTimeExclusive">Returns only orders after specified timestamp (exclusive).</param>
        /// <param name="limitOrderNumber">Maximal number of orders in response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            SortDirection? orderListSorting = null,
            DateTime? untilDateTimeExclusive = null,
            DateTime? afterDateTimeExclusive = null,
            Int64? limitOrderNumber = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "orders",
                Query = new QueryBuilder()
                    .AddParameter("status", orderStatusFilter)
                    .AddParameter("direction", orderListSorting)
                    .AddParameter("until", untilDateTimeExclusive, "O")
                    .AddParameter("after", afterDateTimeExclusive, "O")
                    .AddParameter("limit", limitOrderNumber)
            };

            return _httpClient.GetObjectsListAsync<IOrder, JsonOrder>(
                _alpacaRestApiThrottler, builder, cancellationToken);
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
        /// <param name="orderClass">Order class for advanced order types.</param>
        /// <param name="takeProfitLimitPrice">Profit taking limit price for for advanced order types.</param>
        /// <param name="stopLossStopPrice">Stop loss stop price for for advanced order types.</param>
        /// <param name="stopLossLimitPrice">Stop loss limit price for for advanced order types.</param>
        /// <param name="nested">Whether or not child orders should be listed as 'legs' of parent orders. (Advanced order types only.)</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        public async Task<IOrder> PostOrderAsync(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            Boolean? extendedHours = null,
            OrderClass? orderClass = null,
            Decimal? takeProfitLimitPrice = null,
            Decimal? stopLossStopPrice = null,
            Decimal? stopLossLimitPrice = null,
            Boolean? nested = false,
            CancellationToken cancellationToken = default)
        {
            if (clientOrderId != null &&
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
                ExtendedHours = extendedHours,
                OrderClass = orderClass,
                OrderAttributes = new JsonNewOrderAttributes
                {
                    TakeProfitLimitPrice = takeProfitLimitPrice,
                    StopLossStopPrice = stopLossStopPrice,
                    StopLossLimitPrice = stopLossLimitPrice,
                }
            };

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "orders",
                Query = new QueryBuilder()
                    .AddParameter("nested", nested)
            };

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(newOrder);
            using var response = await _httpClient.PostAsync(
                    new Uri("orders", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IOrder, JsonOrder>()
                .ConfigureAwait(false);
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
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for updated order.</returns>
        public async Task<IOrder> PatchOrderAsync(
            Guid orderId,
            Int64? quantity = null,
            TimeInForce? duration = null,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            CancellationToken cancellationToken = default)
        {
            if (clientOrderId != null &&
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

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var request = new HttpRequestMessage(_httpMethodPatch,
                new Uri($"orders/{orderId:D}", UriKind.RelativeOrAbsolute))
            {
                Content = toStringContent(changeOrder)
            };

            using var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IOrder, JsonOrder>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get single order information by client order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="clientOrderId">Client order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            String clientOrderId,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "orders:by_client_order_id",
                Query = new QueryBuilder()
                    .AddParameter("client_order_id", clientOrderId)
            };

            return _httpClient.GetSingleObjectAsync<IOrder, JsonOrder>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Get single order information by server order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        public Task<IOrder> GetOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"orders/{orderId:D}",
            };

            return _httpClient.GetSingleObjectAsync<IOrder, JsonOrder>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Deletes/cancel order on server by server order ID using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for cancelling.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if order cancellation was accepted.</returns>
        public async Task<Boolean> DeleteOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _httpClient.DeleteAsync(
                    new Uri($"orders/{orderId:D}", UriKind.RelativeOrAbsolute), cancellationToken)
                .ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Deletes/cancel all open orders using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of order cancellation status objects.</returns>
        public async Task<IReadOnlyList<IOrderActionStatus>> DeleteAllOrdersAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "orders",
            };

            return await _httpClient.DeleteObjectsListAsync<IOrderActionStatus, JsonOrderActionStatus>(
                    _alpacaRestApiThrottler, builder, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of position information objects.</returns>
        public Task<IReadOnlyList<IPosition>> ListPositionsAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "positions",
            };

            return _httpClient.GetObjectsListAsync<IPosition, JsonPosition>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only position information object.</returns>
        public Task<IPosition> GetPositionAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + $"positions/{symbol}",
            };

            return _httpClient.GetSingleObjectAsync<IPosition, JsonPosition>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of position cancellation status objects.</returns>
        public async Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "positions",
            };

            return await _httpClient.DeleteObjectsListAsync<IPositionActionStatus, JsonPositionActionStatus>(
                    _alpacaRestApiThrottler, builder, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if position liquidation was accepted.</returns>
        public async Task<Boolean> DeletePositionAsync(
            String symbol,
            CancellationToken cancellationToken = default)
        {
            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _httpClient.DeleteAsync(
                    new Uri($"positions/{symbol}", UriKind.RelativeOrAbsolute), cancellationToken)
                .ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only clock information object.</returns>
        public Task<IClock> GetClockAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "clock",
            };

            return _httpClient.GetSingleObjectAsync<IClock, JsonClock>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDateInclusive">Start time for filtering (inclusive).</param>
        /// <param name="endDateInclusive">End time for filtering (inclusive).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null,
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "calendar",
                Query = new QueryBuilder()
                    .AddParameter("start", startDateInclusive, "yyyy-MM-dd")
                    .AddParameter("end", endDateInclusive, "yyyy-MM-dd")
            };

            return _httpClient.GetObjectsListAsync<ICalendar, JsonCalendar>(
                _alpacaRestApiThrottler, builder, cancellationToken);
        }
    }
}
