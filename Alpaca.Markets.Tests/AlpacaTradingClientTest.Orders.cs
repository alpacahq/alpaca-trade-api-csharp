namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const String OrdersWildcardUrl = $"{OrdersUrlPrefix}/**";

    private const String OrdersUrlPrefix = "/v2/orders";

    private const Decimal FractionalQuantity = 0.35M;

    [Fact]
    public async Task ListOrdersAsyncWorks()
    {
        const Int64 pageSize = 100;

        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        mock.AddGet(OrdersUrlPrefix, new JArray( createOrder()));

        var orders = await mock.Client
            .ListOrdersAsync(new ListOrdersRequest
                {
                    OrderListSorting = SortDirection.Descending,
                    OrderStatusFilter = OrderStatusFilter.Open,
                    RollUpNestedOrders = false,
                    LimitOrderNumber = pageSize
                }
                .WithInterval(new Interval<DateTime>(date, date))
                .WithSymbol(Stock));

        validateOrder(orders.Single());
    }

    [Fact]
    public async Task GetOrderByClientOrderIdAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet($"{OrdersUrlPrefix}:by_client_order_id", createOrder());

        var order = await mock.Client.GetOrderAsync(Guid.NewGuid().ToString("D"));

        validateOrder(order);
    }

    [Fact]
    public async Task GetOrderByServerOrderIdAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet(OrdersWildcardUrl, createOrder());

        var order = await mock.Client.GetOrderAsync(Guid.NewGuid());

        validateOrder(order);
    }

    [Fact]
    public async Task PostRawOrderAsyncWorks()
    {
        const Decimal trailOffsetInDollars = 0.01M;
        const Decimal trailOffsetInPercent = 10M;

        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(
            new NewOrderRequest(Stock, OrderQuantity.Notional(Quantity),
                OrderSide.Buy, OrderType.StopLimit, TimeInForce.Gtc)
            {
                ClientOrderId = Guid.NewGuid().ToString("D"),
                TrailOffsetInDollars = trailOffsetInDollars,
                TrailOffsetInPercent = trailOffsetInPercent,
                OrderClass = OrderClass.OneTriggersOther,
                TakeProfitLimitPrice = BigPrice,
                StopLossLimitPrice = SmallPrice,
                StopLossStopPrice = SmallPrice,
                ExtendedHours = true,
                LimitPrice = BigPrice,
                StopPrice = SmallPrice
            });

        validateOrder(order);
    }

    [Fact]
    public async Task PostBuyOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(
            MarketOrder.Buy(Stock, OrderQuantity.Fractional(FractionalQuantity))
                .WithClientOrderId(Guid.NewGuid().ToString("D"))
                .WithDuration(TimeInForce.Gtc)
                .WithExtendedHours(true));

        validateOrder(order);
    }

    [Fact]
    public async Task PostSellOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(
            MarketOrder.Sell(Stock, OrderQuantity.Fractional(FractionalQuantity))
                .Bracket(BigPrice, SmallPrice));

        validateOrder(order);
    }

    [Fact]
    public async Task PostTakeProfitOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(OrderSide.Buy
            .Limit(Stock, IntegerQuantity, Quantity).TakeProfit(BigPrice));

        validateOrder(order);
    }

    [Fact]
    public async Task PostStopLossOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(OrderSide.Buy
            .Limit(Stock, IntegerQuantity, Quantity).StopLoss(SmallPrice));

        validateOrder(order);
    }

    [Fact]
    public void NewOrderRequestValidationWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        Assert.ThrowsAsync<AggregateException>(() => mock.Client.PostOrderAsync(
            MarketOrder.Buy(String.Empty, OrderQuantity.Fractional(-FractionalQuantity))));
    }

    [Fact]
    public async Task PatchOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPatch(OrdersWildcardUrl, createOrder());

        var order = await mock.Client.PatchOrderAsync(
            new ChangeOrderRequest(Guid.NewGuid())
            {
                Duration = TimeInForce.Day,
                Quantity = IntegerQuantity,
                LimitPrice = BigPrice,
                StopPrice = SmallPrice
            });

        validateOrder(order);
    }

    [Fact]
    public async Task DeleteOrderAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete(OrdersWildcardUrl, createOrder());

        Assert.True(await mock.Client.DeleteOrderAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteAllOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete(OrdersUrlPrefix, new JArray(
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
            new JProperty("filled_qty", Quantity),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("qty", Quantity),
            new JProperty("symbol", Stock),
            new JProperty("legs"));

    private static void validateOrder(
        IOrder order)
    {
        Assert.NotNull(order);

        Assert.NotEqual(Guid.Empty, order.AssetId);
        Assert.NotEqual(Guid.Empty, order.OrderId);
        Assert.Equal(Stock, order.Symbol);

        Assert.Equal(IntegerQuantity, order.IntegerFilledQuantity);
        Assert.Equal(IntegerQuantity, order.IntegerQuantity);
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
