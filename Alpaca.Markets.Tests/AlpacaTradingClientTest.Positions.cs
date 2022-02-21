namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task ListPositionsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions", new JArray(createPosition()));

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

        mock.AddDelete("/v2/positions", getDeletePositionsResponse());

        var statuses = await mock.Client.DeleteAllPositionsAsync();

        Assert.NotNull(statuses);
        Assert.NotEmpty(statuses);

        foreach (var status in statuses)
        {
            Assert.NotNull(status.Symbol);
            Assert.True(status.IsSuccess);
        }
    }

    [Fact]
    public async Task DeleteAllPositionsWithOrdersAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddDelete("/v2/positions", getDeletePositionsResponse());

        var statuses = await mock.Client.DeleteAllPositionsAsync(
            new DeleteAllPositionsRequest
            {
                Timeout = TimeSpan.FromMinutes(5),
                CancelOrders = true
            });

        Assert.NotNull(statuses);
        Assert.NotEmpty(statuses);
    }

    private static JObject createPosition() =>
        new(
            new JProperty("asset_class", AssetClass.UsEquity),
            new JProperty("asset_id", Guid.NewGuid()),
            new JProperty("avg_entry_price", 123.45M),
            new JProperty("exchange", Exchange.Iex),
            new JProperty("cost_basis", 123.45M),
            new JProperty("symbol", Stock),
            new JProperty("qty", 123.45M));

    private static void validatePosition(
        IPosition position)
    {
        Assert.Equal(Stock, position.Symbol);
        Assert.Equal(123, position.IntegerQuantity);
    }

    private static JArray getDeletePositionsResponse() =>
        new (
            new JObject(
                new JProperty("status", (Int64)HttpStatusCode.OK),
                new JProperty("symbol", Stock)),
            new JObject(
                new JProperty("status", (Int64)HttpStatusCode.OK),
                new JProperty("symbol", Stock)));
}
