namespace Alpaca.Markets.Extensions.Tests;

public sealed class BracketTest
{
    private const String Symbol = "AAPL";

    [Fact]
    public void BracketOrderBuyStopLossStopWorks() =>
        assertOrdersAreEqual(
            Bracket.Buy(Symbol, 10, 200M, 100M),
            OrderSide.Buy.Bracket(Symbol, 10, 200M, 100M));

    [Fact]
    public void BracketOrderBuyStopLossStopLimitWorks() =>
        assertOrdersAreEqual(
            Bracket.Buy(Symbol, 10, 200M, 100M, 150M),
            OrderSide.Buy.Bracket(Symbol, 10, 200M, 100M, 150M));

    [Fact]
    public void BracketOrderSellStopLossStopWorks() =>
        assertOrdersAreEqual(
            Bracket.Sell(Symbol, 10, 200M, 100M),
            OrderSide.Sell.Bracket(Symbol, 10, 200M, 100M));

    [Fact]
    public void BracketOrderSellStopLossStopLimitWorks() =>
        assertOrdersAreEqual(
            Bracket.Sell(Symbol, 10, 200M, 100M, 150M),
            OrderSide.Sell.Bracket(Symbol, 10, 200M, 100M, 150M));

    private static void assertOrdersAreEqual(
        BracketOrder lhs,
        BracketOrder rhs)
    {
        Assert.Equal(lhs.Type, rhs.Type);
        Assert.Equal(lhs.Symbol, rhs.Symbol);
        Assert.Equal(lhs.OrderClass, rhs.OrderClass);

        Assert.Equal(lhs.ExtendedHours, rhs.ExtendedHours);
        Assert.Equal(lhs.Duration, rhs.Duration);
        Assert.Equal(lhs.Quantity, rhs.Quantity);
        Assert.Equal(lhs.Side, rhs.Side);

        Assert.NotNull(lhs.TakeProfit);
        Assert.NotNull(rhs.TakeProfit);
        Assert.Equal(lhs.TakeProfit.LimitPrice, rhs.TakeProfit.LimitPrice);

        Assert.NotNull(lhs.StopLoss);
        Assert.NotNull(rhs.StopLoss);
        Assert.Equal(lhs.StopLoss.StopPrice, rhs.StopLoss.StopPrice);
        Assert.Equal(lhs.StopLoss.LimitPrice, rhs.StopLoss.LimitPrice);
    }
}
