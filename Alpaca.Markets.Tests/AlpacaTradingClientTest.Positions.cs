using Xunit;

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
