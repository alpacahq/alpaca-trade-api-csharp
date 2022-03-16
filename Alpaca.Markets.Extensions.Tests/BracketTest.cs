namespace Alpaca.Markets.Extensions.Tests;

public sealed class BracketTest
{
    private const Decimal TakeProfitLimitPrice = 200M;

    private const Decimal StopLossLimitPrice = 150M;

    private const Decimal StopLossStopPrice = 100M;

    private const String Symbol = "AAPL";

    private const Int32 Quantity = 10;

    [Fact]
    public void BracketOrderBuyStopLossStopWorks() =>
        assertOrdersAreEqual(
            Bracket.Buy(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice),
            OrderSide.Buy.Bracket(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice));

    [Fact]
    public void BracketOrderBuyStopLossStopLimitWorks() =>
        assertOrdersAreEqual(
            Bracket.Buy(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice, StopLossLimitPrice),
            OrderSide.Buy.Bracket(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice, StopLossLimitPrice));

    [Fact]
    public void BracketOrderSellStopLossStopWorks() =>
        assertOrdersAreEqual(
            Bracket.Sell(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice),
            OrderSide.Sell.Bracket(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice));

    [Fact]
    public void BracketOrderSellStopLossStopLimitWorks() =>
        assertOrdersAreEqual(
            Bracket.Sell(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice, StopLossLimitPrice),
            OrderSide.Sell.Bracket(Symbol, Quantity, TakeProfitLimitPrice, StopLossStopPrice, StopLossLimitPrice));

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
