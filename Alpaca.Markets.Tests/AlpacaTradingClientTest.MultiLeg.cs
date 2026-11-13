namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const String SoldLegSymbol = "MU260925C00970000";

    private const String BoughtLegSymbol = "MU260925C01000000";

    [Fact]
    public async Task PostMultiLegLimitOrderAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder(),
            "{\"qty\":1.0,\"type\":\"limit\",\"time_in_force\":\"day\",\"limit_price\":-1.60,\"order_class\":\"mleg\","
            + $"\"legs\":[{{\"symbol\":\"{SoldLegSymbol}\",\"ratio_qty\":1.0,\"side\":\"sell\",\"position_intent\":\"sell_to_close\"}},"
            + $"{{\"symbol\":\"{BoughtLegSymbol}\",\"ratio_qty\":1.0,\"side\":\"buy\",\"position_intent\":\"buy_to_close\"}}]}}");

        var order = await mock.Client.PostOrderAsync(
            MultiLegOrder.Limit(OrderQuantity.FromInt64(1), -1.60M,
                OrderSide.Sell.Leg(SoldLegSymbol, 1M, PositionIntent.SellToClose),
                OrderSide.Buy.Leg(BoughtLegSymbol, 1M, PositionIntent.BuyToClose)));

        validateOrder(order);
    }

    [Fact]
    public async Task PostMultiLegMarketOrderAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost(OrdersUrlPrefix, createOrder());

        var order = await mock.Client.PostOrderAsync(
            MultiLegOrder.Market(OrderQuantity.FromInt64(1),
                OrderSide.Sell.Leg(SoldLegSymbol, 1M, PositionIntent.SellToClose),
                OrderSide.Buy.Leg(BoughtLegSymbol, 1M, PositionIntent.BuyToClose)));

        validateOrder(order);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task MultiLegOrderValidationRejectsAnEmptyLegSymbol(String legSymbol)
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        await Assert.ThrowsAsync<RequestValidationException>(() => mock.Client.PostOrderAsync(
            MultiLegOrder.Limit(OrderQuantity.FromInt64(1), 1.00M,
                OrderSide.Buy.Leg(SoldLegSymbol, 1M, PositionIntent.BuyToOpen),
                OrderSide.Sell.Leg(legSymbol, 1M, PositionIntent.SellToOpen))));
    }

    [Fact]
    public void OrderLegRejectsANullSymbol() =>
        Assert.Throws<ArgumentNullException>(() =>
            OrderSide.Sell.Leg(null!, 1M, PositionIntent.SellToOpen));
}
