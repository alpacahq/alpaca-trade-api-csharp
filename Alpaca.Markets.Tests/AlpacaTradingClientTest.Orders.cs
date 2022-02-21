namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/orders", new JArray( createOrder()));

        var orders = await mock.Client
            .ListOrdersAsync(new ListOrdersRequest
                {
                    OrderListSorting = SortDirection.Descending,
                    OrderStatusFilter = OrderStatusFilter.Open,
                    RollUpNestedOrders = false,
                    LimitOrderNumber = 100
                }
                .WithInterval(new Interval<DateTime>())
                .WithSymbol(Stock));

        validateOrder(orders.Single());
    }

    [Fact]
    public async Task GetOrderByClientOrderIdAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/orders:by_client_order_id", createOrder());

        var order = await mock.Client.GetOrderAsync(Guid.NewGuid().ToString("D"));

        validateOrder(order);
    }

    [Fact]
    public async Task GetOrderByServerOrderIdAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/orders/**", createOrder());

        var order = await mock.Client.GetOrderAsync(Guid.NewGuid());

        validateOrder(order);
    }

    [Fact]
    public async Task PostRawOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/orders", createOrder());

        var order = await mock.Client.PostOrderAsync(
            new NewOrderRequest(Stock, OrderQuantity.Notional(12.34M),
                OrderSide.Buy, OrderType.StopLimit, TimeInForce.Gtc)
            {
                ClientOrderId = Guid.NewGuid().ToString("D"),
                OrderClass = OrderClass.OneTriggersOther,
                TrailOffsetInDollars = 0.01M,
                TrailOffsetInPercent = 10M,
                TakeProfitLimitPrice = 1M,
                StopLossLimitPrice = 2M,
                StopLossStopPrice = 3M,
                ExtendedHours = true,
                LimitPrice = 34.56M,
                StopPrice = 78.90M
            });

        validateOrder(order);
    }

    [Fact]
    public async Task PostBuyOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/orders", createOrder());

        var order = await mock.Client.PostOrderAsync(
            MarketOrder.Buy(Stock, OrderQuantity.Fractional(0.55M))
                .WithClientOrderId(Guid.NewGuid().ToString("D"))
                .WithDuration(TimeInForce.Gtc)
                .WithExtendedHours(true));

        validateOrder(order);
    }

    [Fact]
    public async Task PostSellOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/orders", createOrder());

        var order = await mock.Client.PostOrderAsync(
            MarketOrder.Sell(Stock, OrderQuantity.Fractional(0.55M))
                .Bracket(123.45M, 678.90M));

        validateOrder(order);
    }

    [Fact]
    public async Task PostTakeProfitOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/orders", createOrder());

        var order = await mock.Client.PostOrderAsync(OrderSide.Buy
            .Limit(Stock, 42L, 12.34M).TakeProfit(14.15M));

        validateOrder(order);
    }

    [Fact]
    public async Task PostStopLossOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/orders", createOrder());

        var order = await mock.Client.PostOrderAsync(OrderSide.Buy
            .Limit(Stock, 42L, 12.34M).StopLoss(10.11M));

        validateOrder(order);
    }

    [Fact]
    public async Task PatchOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPatch("/v2/orders/**", createOrder());

        var order = await mock.Client.PatchOrderAsync(
            new ChangeOrderRequest(Guid.NewGuid())
            {
                Duration = TimeInForce.Day,
                Quantity = 12345L,
                LimitPrice = 12M,
                StopPrice = 34M
            });

        validateOrder(order);
    }

    [Fact]
    public async Task DeleteOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/orders/**", createOrder());

        Assert.True(await mock.Client.DeleteOrderAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAllOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/orders", new JArray(
            new JObject(
                new JProperty("status", (Int64)HttpStatusCode.OK),
                new JProperty("id", Guid.NewGuid()))));

        var statuses = await mock.Client.DeleteAllOrdersAsync();

        var status = statuses.Single();

        Assert.NotEqual(Guid.Empty, status.OrderId);
        Assert.True(status.IsSuccess);
    }

    private static JToken createOrder() =>
        new JObject(
            new JProperty("status", OrderStatus.PartiallyFilled),
            new JProperty("asset_class", AssetClass.UsEquity),
            new JProperty("time_in_force", TimeInForce.Day),
            new JProperty("order_class", OrderClass.Simple),
            new JProperty("asset_id", Guid.NewGuid()),
            new JProperty("type", OrderType.Market),
            new JProperty("side", OrderSide.Sell),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("filled_qty", 56.43M),
            new JProperty("qty", 1234.56M),
            new JProperty("symbol", Stock),
            new JProperty("legs"));

    private static void validateOrder(
        IOrder order)
    {
        Assert.NotNull(order);

        Assert.NotEqual(Guid.Empty, order.AssetId);
        Assert.NotEqual(Guid.Empty, order.OrderId);
        Assert.Equal(Stock, order.Symbol);

        Assert.Equal(56L, order.IntegerFilledQuantity);
        Assert.Equal(1235L, order.IntegerQuantity);
        Assert.True(order.GetOrderQuantity().IsInShares);

        Assert.Null(order.TrailOffsetInPercent);
        Assert.Null(order.TrailOffsetInDollars);
        Assert.Null(order.ReplacedByOrderId);
        Assert.Null(order.AverageFillPrice);
        Assert.Null(order.ReplacesOrderId);
        Assert.Null(order.ClientOrderId);
        Assert.Null(order.HighWaterMark);
        Assert.Null(order.LimitPrice);
        Assert.Null(order.StopPrice);
        Assert.Null(order.Notional);

        Assert.Null(order.SubmittedAtUtc);
        Assert.Null(order.CancelledAtUtc);
        Assert.Null(order.ReplacedAtUtc);
        Assert.Null(order.CreatedAtUtc);
        Assert.Null(order.UpdatedAtUtc);
        Assert.Null(order.ExpiredAtUtc);
        Assert.Null(order.FilledAtUtc);
        Assert.Null(order.FailedAtUtc);

        Assert.Empty(order.Legs);
    }
}
