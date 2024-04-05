namespace Alpaca.Markets;

internal sealed partial class AlpacaTradingClient
{
    public async Task<IReadOnlyList<IOrder>> ListOrdersAsync(
        ListOrdersRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IReadOnlyList<IOrder>, List<JsonOrder>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IOrder> PostOrderAsync(
        NewOrderRequest request,
        CancellationToken cancellationToken = default) =>
        postOrderAsync(request.EnsureNotNull().Validate().GetJsonRequest(), cancellationToken);

    public Task<IOrder> PostOrderAsync(
        OrderBase orderBase,
        CancellationToken cancellationToken = default) =>
        postOrderAsync(orderBase.EnsureNotNull().Validate().GetJsonRequest(), cancellationToken);

    private Task<IOrder> postOrderAsync(
        JsonNewOrder jsonNewOrder,
        CancellationToken cancellationToken = default) =>
        _httpClient.PostAsync<IOrder, JsonOrder, JsonNewOrder>(
            "v2/orders", jsonNewOrder, _rateLimitHandler, cancellationToken);

    public Task<IOrder> PatchOrderAsync(
        ChangeOrderRequest request,
        CancellationToken cancellationToken = default) =>
        _httpClient.PatchAsync<IOrder, JsonOrder, ChangeOrderRequest>(
            request.EnsureNotNull().Validate().GetEndpointUri(),
            request, _rateLimitHandler, cancellationToken);

    public async Task<IOrder> GetOrderAsync(
        String clientOrderId,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IOrder, JsonOrder>(
            new UriBuilder(_httpClient.BaseAddress!)
            {
                Path = "v2/orders:by_client_order_id",
                Query = await new QueryBuilder()
                    .AddParameter("client_order_id", clientOrderId)
                    .AsStringAsync().ConfigureAwait(false)
            },
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IOrder> GetOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IOrder, JsonOrder>(
            $"v2/orders/{orderId:D}", _rateLimitHandler, cancellationToken);

    public Task<Boolean> CancelOrderAsync(
        Guid orderId,
        CancellationToken cancellationToken = default) =>
        _httpClient.TryDeleteAsync(
            $"v2/orders/{orderId:D}", _rateLimitHandler, cancellationToken);

    public Task<IReadOnlyList<IOrderActionStatus>> CancelAllOrdersAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.DeleteAsync<IReadOnlyList<IOrderActionStatus>, List<JsonOrderActionStatus>>(
            "v2/orders", _rateLimitHandler, cancellationToken);

    public Task<Boolean> ExerciseOptionsPositionByIdAsync(
        Guid contractId,
        CancellationToken cancellationToken = default) =>
        _httpClient.TryPostAsync(
            $"v2/positions/{contractId:D}/exercise", _rateLimitHandler, cancellationToken);

    public Task<Boolean> ExerciseOptionsPositionBySymbolAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        _httpClient.TryPostAsync(
            $"v2/positions/{symbol.EnsureNotNull()}/exercise", _rateLimitHandler, cancellationToken);
}
