using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListPositionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions", new JsonPosition[]
        {
            new ()
            {
                Quantity = 123.45M,
                Symbol = Stock
            }
        });

        var positions = await mock.Client.ListPositionsAsync();

        var position = positions.Single();
        Assert.Equal(Stock, position.Symbol);
        Assert.Equal(123, position.IntegerQuantity);
    }

    [Fact]
    public async Task GetPositionAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions/**", new JsonPosition
        {
            Quantity = 123.45M,
            Symbol = Stock
        });

        var position = await mock.Client.GetPositionAsync(Stock);

        Assert.Equal(Stock, position.Symbol);
        Assert.Equal(123, position.IntegerQuantity);
    }
}
