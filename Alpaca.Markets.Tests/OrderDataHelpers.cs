using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Global")]
internal static class OrderDataHelpers
{
    private const Int64 IntegerQuantity = 123L;

    private const Decimal Quantity = 123.45M;

    public static JToken CreateMarketOrder(
        this String symbol) =>
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
            new JProperty("symbol", symbol),
            new JProperty("qty", Quantity),
            new JProperty("legs"));
    
    public static void Validate(
        this IOrder order,
        String symbol)
    {
        Assert.NotNull(order);

        Assert.NotEqual(Guid.Empty, order.AssetId);
        Assert.NotEqual(Guid.Empty, order.OrderId);
        Assert.Equal(symbol, order.Symbol);

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
