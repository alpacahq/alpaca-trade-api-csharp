using System.Net;
using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/orders", new [] { createOrder() });

        var orders = await mock.Client
            .ListOrdersAsync(new ListOrdersRequest());

        validateOrder(orders.Single());
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
                .Bracket(123.45M, 678.90M, 612.34M));

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

        mock.AddDelete("/v2/orders", new JsonOrderActionStatus[]
        {
            new ()
            {
                StatusCode = (Int64)HttpStatusCode.OK,
                OrderId = Guid.NewGuid()
            }
        });

        var statuses = await mock.Client.DeleteAllOrdersAsync();

        var status = statuses.Single();
        Assert.True(status.IsSuccess);
    }

    private static JsonOrder createOrder() =>
        new ()
        {
            FilledQuantity = 56.43M,
            Quantity = 1234.56M,
        };

    private static void validateOrder(
        IOrder order)
    {
        Assert.NotNull(order);
        Assert.Empty(order.Legs);
        Assert.Equal(1235L, order.IntegerQuantity);
        Assert.Equal(56L, order.IntegerFilledQuantity);
    }
}
