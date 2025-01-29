namespace Alpaca.Markets;

/// <summary>
/// TBD
/// </summary>
public sealed class MultiLegOrder : AdvancedOrderBase
{
    private readonly List<OrderLeg> _legs;

    private MultiLegOrder(
        SimpleOrderBase baseOrder,
        params IReadOnlyList<OrderLeg> legs)
        : base(baseOrder, OrderClass.MultiLegOptions)
    {
        Duration = TimeInForce.Day;
        _legs = [.. legs];
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> market order with two legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Market(
        OrderQuantity quantity,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2) =>
        createMarket(quantity, orderLeg1, orderLeg2);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> market order with three legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <param name="orderLeg3">Third leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Market(
        OrderQuantity quantity,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2,
        OrderLeg orderLeg3) =>
        createMarket(quantity, orderLeg1, orderLeg2, orderLeg3);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> market order with four legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <param name="orderLeg3">Third leg of the multi-leg order.</param>
    /// <param name="orderLeg4">Fourth leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Market(
        OrderQuantity quantity,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2,
        OrderLeg orderLeg3,
        OrderLeg orderLeg4) =>
        createMarket(quantity, orderLeg1, orderLeg2, orderLeg3, orderLeg4);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> limit order with two legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Limit(
        OrderQuantity quantity,
        Decimal limitPrice,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2) =>
        createLimit(quantity, limitPrice, orderLeg1, orderLeg2);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> limit order with three legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <param name="orderLeg3">Third leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Limit(
        OrderQuantity quantity,
        Decimal limitPrice,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2,
        OrderLeg orderLeg3) =>
        createLimit(quantity, limitPrice, orderLeg1, orderLeg2, orderLeg3);

    /// <summary>
    /// Creates a new instance of the <see cref="MultiLegOrder"/> limit order with four legs.
    /// </summary>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="limitPrice">Order limit price.</param>
    /// <param name="orderLeg1">First leg of the multi-leg order.</param>
    /// <param name="orderLeg2">Second leg of the multi-leg order.</param>
    /// <param name="orderLeg3">Third leg of the multi-leg order.</param>
    /// <param name="orderLeg4">Fourth leg of the multi-leg order.</param>
    /// <returns>A new advanced options order with several legs.</returns>
    public static MultiLegOrder Limit(
        OrderQuantity quantity,
        Decimal limitPrice,
        OrderLeg orderLeg1,
        OrderLeg orderLeg2,
        OrderLeg orderLeg3,
        OrderLeg orderLeg4) =>
        createLimit(quantity, limitPrice, orderLeg1, orderLeg2, orderLeg3, orderLeg4);

    private static MultiLegOrder createLimit(
        OrderQuantity quantity,
        Decimal limitPrice,
        params IReadOnlyList<OrderLeg> legs) =>
        new(new LimitOrder(String.Empty, quantity, OrderSide.Buy, limitPrice), legs);

    private static MultiLegOrder createMarket(
        OrderQuantity quantity,
        params IReadOnlyList<OrderLeg> legs) =>
        new(new MarketOrder(String.Empty, quantity, OrderSide.Buy), legs);

    internal override JsonNewOrder GetJsonRequest() =>
        base.GetJsonRequest()
            .WithOrderLegs(_legs.Select(leg => leg.GetJsonRequest()))
            .WithoutOrderSide()
            .WithoutSymbol();
}