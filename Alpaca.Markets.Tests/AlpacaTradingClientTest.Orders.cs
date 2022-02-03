using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/orders", new JsonOrder[]
        {
            new()
            {
                FilledQuantity = 56.43M,
                Quantity = 1234.56M,
            }
        });

        var orders = await mock.Client
            .ListOrdersAsync(new ListOrdersRequest());

        var order = orders.Single();

        Assert.NotNull(order);
        Assert.Empty(order.Legs);
        Assert.Equal(1235L, order.IntegerQuantity);
        Assert.Equal(56L, order.IntegerFilledQuantity);
    }
}
