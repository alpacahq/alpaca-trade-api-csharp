namespace Alpaca.Markets.Tests;

public  sealed class OrderTypeTest
{
    private const String Stock = "AAPL";

    [Fact]
    public void MarketOrderCreationWorks()
    {
        assertOrdersAreEqual(
            MarketOrder.Buy(Stock, 10L),
            OrderSide.Buy.Market(Stock, 10L));

        assertOrdersAreEqual(
            MarketOrder.Sell(Stock, 10L),
            OrderSide.Sell.Market(Stock, 10L));
    }

    [Fact]
    public void TrailingStopOrderCreationWorks()
    {
        var offsetInDollars = TrailOffset.InDollars(10M);
        var offsetInPercent = TrailOffset.InPercent(10M);

        assertOrdersAreEqual(
            TrailingStopOrder.Buy(Stock, 10L, offsetInDollars),
            OrderSide.Buy.TrailingStop(Stock, 10L, offsetInDollars));

        assertOrdersAreEqual(
            TrailingStopOrder.Sell(Stock, 10L, offsetInPercent),
            OrderSide.Sell.TrailingStop(Stock, 10L, offsetInPercent));
    }

    [Fact]
    public void LimitOrderCreationWorks()
    {
        const Decimal limitPrice = 100M;

        assertOrdersAreEqual(
            LimitOrder.Buy(Stock, 10L, limitPrice),
            OrderSide.Buy.Limit(Stock, 10L, limitPrice));

        assertOrdersAreEqual(
            LimitOrder.Sell(Stock, 10L, limitPrice),
            OrderSide.Sell.Limit(Stock, 10L, limitPrice));
    }

    [Fact]
    public void StopOrderCreationWorks()
    {
        const Decimal stopPrice = 100M;

        assertOrdersAreEqual(
            StopOrder.Buy(Stock, 10L, stopPrice),
            OrderSide.Buy.Stop(Stock, 10L, stopPrice));

        assertOrdersAreEqual(
            StopOrder.Sell(Stock, 10L, stopPrice),
            OrderSide.Sell.Stop(Stock, 10L, stopPrice));
    }

    [Fact]
    public void StopLimitOrderCreationWorks()
    {
        const Decimal stopPrice = 100M;
        const Decimal limitPrice = 200M;

        assertOrdersAreEqual(
            StopLimitOrder.Buy(Stock, 10L, stopPrice, limitPrice),
            OrderSide.Buy.StopLimit(Stock, 10L, stopPrice, limitPrice));

        assertOrdersAreEqual(
            StopLimitOrder.Sell(Stock, 10L, stopPrice, limitPrice),
            OrderSide.Sell.StopLimit(Stock, 10L, stopPrice, limitPrice));
    }

    [Fact]
    public void OneCancelsOtherOrderCreationWorks()
    {
        const Decimal takeProfitPrice = 130M;
        const Decimal stopLossStopPrice = 110M;
        const Decimal stopLossLimitPrice = 120M;

        assertOrdersAreEqual(
            LimitOrder.Buy(Stock, 100L, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice, stopLossLimitPrice),
            OrderSide.Buy.Limit(Stock, 100L, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice, stopLossLimitPrice));

        assertOrdersAreEqual(
            LimitOrder.Sell(Stock, 100L, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice),
            OrderSide.Sell.Limit(Stock, 100L, takeProfitPrice)
                .OneCancelsOther(stopLossStopPrice));
    }

    [Fact]
    public void BracketOrderCreationWorks()
    {
        const Decimal stopLossPrice = 100M;
        const Decimal takeProfitPrice = 130M;
        const Decimal stopLossStopPrice = 110M;
        const Decimal stopLossLimitPrice = 120M;

        var marketOrder = MarketOrder.Buy(Stock, 100L);

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
        MarketOrder lhs,
        MarketOrder rhs)
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
            JsonConvert.SerializeObject(lhs.GetJsonRequest()),
            JsonConvert.SerializeObject(rhs.GetJsonRequest()));
}
