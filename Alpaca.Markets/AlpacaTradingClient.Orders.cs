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
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IReadOnlyList<IOrder>> ListAllOrdersAsync(
            CancellationToken cancellationToken = default) =>
            // TODO: olegra - remove this overload after removing old version with separate arguments
            ListOrdersAsync(new ListOrdersRequest(), cancellationToken);

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">List orders request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        public Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            ListOrdersRequest request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = _httpClient.BaseAddress.AbsolutePath + "orders",
                Query = new QueryBuilder()
                    .AddParameter("status", request.OrderStatusFilter)
                    .AddParameter("direction", request.OrderListSorting)
                    .AddParameter("until", request.UntilDateTimeExclusive, "O")
                    .AddParameter("after", request.AfterDateTimeExclusive, "O")
                    .AddParameter("limit", request.LimitOrderNumber)
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
        /// <param name="takeProfitLimitPrice">Profit taking limit price for advanced order types.</param>
        /// <param name="stopLossStopPrice">Stop loss stop price for advanced order types.</param>
        /// <param name="stopLossLimitPrice">Stop loss limit price for advanced order types.</param>
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

            JsonNewOrderAdvancedAttributes? takeProfit = null, stopLoss = null;
            if (takeProfitLimitPrice != null)
            {
                takeProfit = new JsonNewOrderAdvancedAttributes
                {
                    LimitPrice = takeProfitLimitPrice
                };
            }

            if (stopLossStopPrice != null || stopLossLimitPrice != null)
            {
                stopLoss = new JsonNewOrderAdvancedAttributes
                {
                    StopPrice = stopLossStopPrice,
                    LimitPrice = stopLossLimitPrice
                };
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
                TakeProfit = takeProfit,
                StopLoss = stopLoss,
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
                    builder.Uri, content, cancellationToken)
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
    }
}