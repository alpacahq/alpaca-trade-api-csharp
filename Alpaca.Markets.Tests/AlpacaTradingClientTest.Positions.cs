using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const Decimal ProfitLossPercent = 0.42M;

    private const Int64 IntegerQuantity = 123L;

    private const Decimal SmallPrice = 123.45M;

    private const Decimal BigPrice = 234.56M;

    private const Decimal Quantity = 123.45M;

    private const Decimal ProfitLoss = 42M;

    [Fact]
    public async Task ListPositionsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions", new JArray(createPosition()));

        var positions = await mock.Client.ListPositionsAsync();

        validatePosition(positions.Single());
    }

    [Fact]
    public async Task GetPositionAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/positions/**", createPosition());

        var position = await mock.Client.GetPositionAsync(Stock);

        validatePosition(position);
    }

    [Fact]
    public async Task DeletePositionAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

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
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

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
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

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

    [Fact]
    public async Task ExerciseOptionsPositionByIdAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/positions/*/exercise", new JObject());

        Assert.True(await mock.Client.ExerciseOptionsPositionByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task ExerciseOptionsPositionBySymbolAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddPost("/v2/positions/*/exercise", new JObject());

        Assert.True(await mock.Client.ExerciseOptionsPositionBySymbolAsync(Stock));
    }

#pragma warning disable IDE0079 // Remove unnecessary suppression
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
#pragma warning restore IDE0079 // Remove unnecessary suppression
    private static JObject createPosition() =>
        new(
            new JProperty("unrealized_intraday_plpc", ProfitLossPercent),
            new JProperty("unrealized_plpc", ProfitLossPercent),
            new JProperty("unrealized_intraday_pl", ProfitLoss),
            new JProperty("asset_class", AssetClass.UsEquity),
            new JProperty("avg_entry_price", SmallPrice),
            new JProperty("unrealized_pl", ProfitLoss),
            new JProperty("asset_id", Guid.NewGuid()),
            new JProperty("market_value", SmallPrice),
            new JProperty("side", PositionSide.Long),
            new JProperty("current_price", BigPrice),
            new JProperty("lastday_price", BigPrice),
            new JProperty("qty_available", Quantity),
            new JProperty("change_today", BigPrice),
            new JProperty("cost_basis", SmallPrice),
            new JProperty("exchange", Exchange.Iex),
            new JProperty("qty", Quantity),
            new JProperty("symbol", Stock));

    private static void validatePosition(
        IPosition position)
    {
        Assert.Equal(Stock, position.Symbol);

        Assert.Equal(IntegerQuantity, position.IntegerAvailableQuantity);
        Assert.Equal(IntegerQuantity, position.IntegerQuantity);
        Assert.Equal(PositionSide.Long, position.Side);

        Assert.NotEqual(Guid.Empty, position.AssetId);

        Assert.NotEqual(0M, position.AssetChangePercent);
        Assert.NotEqual(0M, position.AverageEntryPrice);
        Assert.NotEqual(0M, position.AssetCurrentPrice);
        Assert.NotEqual(0M, position.AssetLastPrice);
        Assert.NotEqual(0M, position.MarketValue);
        Assert.NotEqual(0M, position.CostBasis);

        Assert.NotEqual(0M, position.UnrealizedProfitLoss);
        Assert.NotEqual(0M, position.UnrealizedProfitLossPercent);
        Assert.NotEqual(0M, position.IntradayUnrealizedProfitLoss);
        Assert.NotEqual(0M, position.IntradayUnrealizedProfitLossPercent);
    }

    private static JArray getDeletePositionsResponse() =>
        new(
            new JObject(
                new JProperty("status", (Int64)HttpStatusCode.OK),
                new JProperty("symbol", Stock)),
            new JObject(
                new JProperty("status", (Int64)HttpStatusCode.OK),
                new JProperty("symbol", Stock)));
}
