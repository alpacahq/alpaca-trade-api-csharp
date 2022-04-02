using System.Reflection;

namespace Alpaca.Markets.Tests;

public  sealed class OrderTypeTest
{
    private const String Stock = "AAPL";

    private const Int64 Quantity = 10L;

    private static readonly MethodInfo? _getJsonRequest = typeof(OrderBase).GetMethod(
        "GetJsonRequest", BindingFlags.Instance | BindingFlags.NonPublic);

    [Fact]
    public void MarketOrderCreationWorks()
    {
        assertOrdersAreEqual(
            MarketOrder.Buy(Stock, Quantity),
            OrderSide.Buy.Market(Stock, Quantity));

        assertOrdersAreEqual(
            MarketOrder.Sell(Stock, Quantity),
            OrderSide.Sell.Market(Stock, Quantity));
    }

    [Fact]
    public void TrailingStopOrderCreationWorks()
    {
        var offsetInDollars = TrailOffset.InDollars(10M);
        var offsetInPercent = TrailOffset.InPercent(10M);

        assertOrdersAreEqual(
            TrailingStopOrder.Buy(Stock, Quantity, offsetInDollars),
            OrderSide.Buy.TrailingStop(Stock, Quantity, offsetInDollars));

        assertOrdersAreEqual(
            TrailingStopOrder.Sell(Stock, Quantity, offsetInPercent),
            OrderSide.Sell.TrailingStop(Stock, Quantity, offsetInPercent));
    }

    [Fact]
    public void LimitOrderCreationWorks()
    {
        const Decimal limitPrice = 100M;

        assertOrdersAreEqual(
            LimitOrder.Buy(Stock, Quantity, limitPrice),
            OrderSide.Buy.Limit(Stock, Quantity, limitPrice));

        assertOrdersAreEqual(
            LimitOrder.Sell(Stock, Quantity, limitPrice),
            OrderSide.Sell.Limit(Stock, Quantity, limitPrice));
    }

    [Fact]
    public void StopOrderCreationWorks()
    {
        const Decimal stopPrice = 100M;

        assertOrdersAreEqual(
            StopOrder.Buy(Stock, Quantity, stopPrice),
            OrderSide.Buy.Stop(Stock, Quantity, stopPrice));

        assertOrdersAreEqual(
            StopOrder.Sell(Stock, Quantity, stopPrice),
            OrderSide.Sell.Stop(Stock, Quantity, stopPrice));
    }

    [Fact]
    public void StopLimitOrderCreationWorks()
    {
        const Decimal stopPrice = 100M;
        const Decimal limitPrice = 200M;

        assertOrdersAreEqual(
            StopLimitOrder.Buy(Stock, Quantity, stopPrice, limitPrice),
            OrderSide.Buy.StopLimit(Stock, Quantity, stopPrice, limitPrice));

        assertOrdersAreEqual(
            StopLimitOrder.Sell(Stock, Quantity, stopPrice, limitPrice),
            OrderSide.Sell.StopLimit(Stock, Quantity, stopPrice, limitPrice));
    }

    [Fact]
    public void OneCancelsOtherOrderCreationWorks()
    {
        const Decimal takeProfitPrice = 130M;
        const Decimal stopLossStopPrice = 110M;
        const Decimal stopLossLimitPrice = 120M;

        assertOrdersAreEqual(
            LimitOrder.Buy(Stock, Quantity, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice, stopLossLimitPrice),
            OrderSide.Buy.Limit(Stock, Quantity, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice, stopLossLimitPrice));

        assertOrdersAreEqual(
            LimitOrder.Sell(Stock, Quantity, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice),
            OrderSide.Sell.Limit(Stock, Quantity, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice));
    }

    [Fact]
    public void BracketOrderCreationWorks()
    {
        const Decimal stopLossPrice = 100M;
        const Decimal takeProfitPrice = 130M;
        const Decimal stopLossStopPrice = 110M;
        const Decimal stopLossLimitPrice = 120M;

        var marketOrder = MarketOrder.Buy(Stock, Quantity);

        var stopLossOrder = marketOrder.StopLoss(stopLossPrice);
        var takeProfitOrder = marketOrder.TakeProfit(takeProfitPrice);

        assertOrdersAreEqual(
            stopLossOrder.TakeProfit(takeProfitPrice),
            takeProfitOrder.StopLoss(stopLossPrice));

        assertOrdersAreEqual(
            marketOrder.Bracket(
                takeProfitPrice, stopLossStopPrice, stopLossLimitPrice),
            marketOrder.TakeProfit(takeProfitPrice)
                .StopLoss(stopLossStopPrice, stopLossLimitPrice));
    }

    private static void assertOrdersAreEqual(
        OrderBase lhs,
        OrderBase rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);

        Assert.Equal(lhs.Quantity, rhs.Quantity);

        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        TrailingStopOrder lhs,
        TrailingStopOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);

        Assert.Equal(lhs.TrailOffset, rhs.TrailOffset);
        Assert.Equal(lhs.TrailOffset.IsInDollars, rhs.TrailOffset.IsInDollars);
        Assert.Equal(lhs.TrailOffset.IsInPercent, rhs.TrailOffset.IsInPercent);

        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        LimitOrder lhs,
        LimitOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);

        Assert.Equal(lhs.LimitPrice, rhs.LimitPrice);

        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        StopOrder lhs,
        StopOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);

        Assert.Equal(lhs.StopPrice, rhs.StopPrice);

        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        StopLimitOrder lhs,
        StopLimitOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);

        Assert.Equal(lhs.LimitPrice, rhs.LimitPrice);
        Assert.Equal(lhs.StopPrice, rhs.StopPrice);

        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        OneCancelsOtherOrder lhs,
        OneCancelsOtherOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);
        assertOrdersAreEqual(lhs.TakeProfit, rhs.TakeProfit);
        assertOrdersAreEqual(lhs.StopLoss, rhs.StopLoss);
        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        BracketOrder lhs,
        BracketOrder rhs)
    {
        assertOrderBasePropertiesAreEqual(lhs, rhs);
        assertOrdersAreEqual(lhs.TakeProfit, rhs.TakeProfit);
        assertOrdersAreEqual(lhs.StopLoss, rhs.StopLoss);
        assertJsonSerializedOrdersAreEqual(lhs, rhs);
    }

    private static void assertOrdersAreEqual(
        ITakeProfit lhs,
        ITakeProfit rhs) =>
        Assert.Equal(lhs.LimitPrice, rhs.LimitPrice);

    private static void assertOrdersAreEqual(
        IStopLoss lhs,
        IStopLoss rhs)
    {
        Assert.Equal(lhs.LimitPrice, rhs.LimitPrice);
        Assert.Equal(lhs.StopPrice, rhs.StopPrice);
    }

    private static void assertOrderBasePropertiesAreEqual(
        OrderBase lhs,
        OrderBase rhs)
    {
        Assert.Equal(lhs.Quantity, rhs.Quantity);
        Assert.Equal(lhs.Duration, rhs.Duration);
        Assert.Equal(lhs.Symbol, rhs.Symbol);
        Assert.Equal(lhs.Side, rhs.Side);
        Assert.Equal(lhs.Type, rhs.Type);
    }

    private static void assertJsonSerializedOrdersAreEqual(
        OrderBase lhs,
        OrderBase rhs) =>
        Assert.Equal(
            JsonConvert.SerializeObject(getJsonRequest(lhs)),
            JsonConvert.SerializeObject(getJsonRequest(rhs)));

    private static Object? getJsonRequest(
        OrderBase order) =>
        _getJsonRequest?.Invoke(order, null);
}
