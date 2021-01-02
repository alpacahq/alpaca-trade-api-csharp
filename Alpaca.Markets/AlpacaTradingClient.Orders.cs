using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <inheritdoc />
        public Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            ListOrdersRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IOrder>, List<JsonOrder>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IOrder> PostOrderAsync(
            NewOrderRequest request,
            CancellationToken cancellationToken = default) =>
            postOrderAsync(request.EnsureNotNull(nameof(request)).Validate().GetJsonRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<IOrder> PostOrderAsync(
            OrderBase orderBase,
            CancellationToken cancellationToken = default) =>
            postOrderAsync(orderBase.EnsureNotNull(nameof(orderBase)).Validate().GetJsonRequest(), cancellationToken);

        private Task<IOrder> postOrderAsync(
            JsonNewOrder jsonNewOrder,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsync<IOrder, JsonOrder, JsonNewOrder>(
                "v2/orders", jsonNewOrder, cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IOrder> PatchOrderAsync(
            ChangeOrderRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.PatchAsync<IOrder, JsonOrder, ChangeOrderRequest>(
                request.EnsureNotNull(nameof(request)).Validate().GetEndpointUri(),
                request, _alpacaRestApiThrottler, cancellationToken);

        /// <inheritdoc />
        public Task<IOrder> GetOrderAsync(
            String clientOrderId,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IOrder, JsonOrder>(
                new UriBuilder(_httpClient.BaseAddress!)
                {
                    Path = "v2/orders:by_client_order_id",
                    Query = new QueryBuilder()
                        .AddParameter("client_order_id", clientOrderId)
                },
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IOrder> GetOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IOrder, JsonOrder>(
                $"v2/orders/{orderId:D}", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<Boolean> DeleteOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync(
                $"v2/orders/{orderId:D}", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyList<IOrderActionStatus>> DeleteAllOrdersAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IReadOnlyList<IOrderActionStatus>, List<JsonOrderActionStatus>>(
                    "v2/orders", cancellationToken, _alpacaRestApiThrottler);
    }
}
