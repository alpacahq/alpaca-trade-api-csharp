namespace Alpaca.Markets;

internal static class JsonNewOrderExtensions
{
    public static JsonNewOrder WithoutLimitPrice(
        this JsonNewOrder order)
    {
        order.LimitPrice = null;
        return order;
    }

    public static JsonNewOrder WithStopPrice(
        this JsonNewOrder order,
        Decimal stopPrice)
    {
        order.StopPrice = stopPrice;
        return order;
    }

    public static JsonNewOrder WithLimitPrice(
        this JsonNewOrder order,
        Decimal limitPrice)
    {
        order.LimitPrice = limitPrice;
        return order;
    }

    public static JsonNewOrder WithTrailOffset(
        this JsonNewOrder order,
        TrailOffset trailOffset)
    {
        if (trailOffset.IsInDollars)
        {
            order.TrailOffsetInDollars = trailOffset.Value;
        }
        else
        {
            order.TrailOffsetInPercent = trailOffset.Value;
        }
        return order;
    }

    public static JsonNewOrder WithOrderClass(
        this JsonNewOrder order,
        OrderClass orderClass)
    {
        order.OrderClass = orderClass;
        return order;
    }

    public static JsonNewOrder WithTakeProfit(
        this JsonNewOrder order,
        ITakeProfit takeProfit)
    {
        order.TakeProfit = new JsonNewOrderAdvancedAttributes
        {
            LimitPrice = takeProfit.LimitPrice
        };
        return order;
    }

    public static JsonNewOrder WithStopLoss(
        this JsonNewOrder order,
        IStopLoss stopLoss)
    {
        order.StopLoss = new JsonNewOrderAdvancedAttributes
        {
            StopPrice = stopLoss.StopPrice,
            LimitPrice = stopLoss.LimitPrice
        };
        return order;
    }
}
