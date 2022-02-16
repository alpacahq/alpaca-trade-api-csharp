namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListPositionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions", new [] { createPosition() });

        var positions = await mock.Client.ListPositionsAsync();

        validatePosition(positions.Single());
    }

    [Fact]
    public async Task GetPositionAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions/**", createPosition());

        var position = await mock.Client.GetPositionAsync(Stock);

        validatePosition(position);
    }

    [Fact]
    public async Task DeletePositionAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/positions/**", createOrder());

        var order = await mock.Client.DeletePositionAsync(
            new DeletePositionRequest(Stock)
            {
                PositionQuantity = PositionQuantity.InPercents(50)
            });

        validateOrder(order);
    }

    [Fact]
    public async Task DeleteAllPositionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/positions", new JsonPositionActionStatus []
        {
            new () { StatusCode = (Int64)HttpStatusCode.OK, Symbol = Stock },
            new () { StatusCode = (Int64)HttpStatusCode.OK, Symbol = Crypto }
        });

        var statuses = await mock.Client.DeleteAllPositionsAsync();

        Assert.NotNull(statuses);
        Assert.NotEmpty(statuses);
        foreach (var status in statuses)
        {
            Assert.True(status.IsSuccess);
        }
    }

    [Fact]
    public async Task DeleteAllPositionsWithOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/positions", new JsonPositionActionStatus []
        {
            new () { StatusCode = (Int64)HttpStatusCode.OK, Symbol = Stock },
            new () { StatusCode = (Int64)HttpStatusCode.OK, Symbol = Crypto }
        });

        var statuses = await mock.Client.DeleteAllPositionsAsync(
            new DeleteAllPositionsRequest
            {
                Timeout = TimeSpan.FromMinutes(5),
                CancelOrders = true
            });

        Assert.NotNull(statuses);
        Assert.NotEmpty(statuses);
    }

    private static JsonPosition createPosition() =>
        new ()
        {
            Quantity = 123.45M,
            Symbol = Stock
        };

    private static void validatePosition(IPosition position)
    {
        Assert.Equal(Stock, position.Symbol);
        Assert.Equal(123, position.IntegerQuantity);
    }
}
