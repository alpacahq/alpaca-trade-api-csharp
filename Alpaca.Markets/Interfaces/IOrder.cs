namespace Alpaca.Markets;

/// <summary>
/// Encapsulates order information from Alpaca REST API.
/// </summary>
[CLSCompliant(false)]
public interface IOrder
{
    /// <summary>
    /// Gets unique server-side order identifier.
    /// </summary>
    [UsedImplicitly]
    Guid OrderId { get; }

    /// <summary>
    /// Gets client-side (user specified) order identifier. Client Order IDs must be unique.
    /// </summary>
    [UsedImplicitly]
    String? ClientOrderId { get; }

    /// <summary>
    /// Gets order creation timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? CreatedAtUtc { get; }

    /// <summary>
    /// Gets last order update timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? UpdatedAtUtc { get; }

    /// <summary>
    /// Gets order submission timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? SubmittedAtUtc { get; }

    /// <summary>
    /// Gets order fill timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? FilledAtUtc { get; }

    /// <summary>
    /// Gets order expiration timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? ExpiredAtUtc { get; }

    /// <summary>
    /// Gets order cancellation timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? CancelledAtUtc { get; }

    /// <summary>
    /// Gets order rejection timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? FailedAtUtc { get; }

    /// <summary>
    /// Gets order replacement timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? ReplacedAtUtc { get; }

    /// <summary>
    /// Gets unique asset identifier.
    /// </summary>
    [UsedImplicitly]
    Guid AssetId { get; }

    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets asset class.
    /// </summary>
    [UsedImplicitly]
    AssetClass AssetClass { get; }

    /// <summary>
    /// Gets original notional order quantity (with the fractional part).
    /// </summary>
    Decimal? Notional { get; }

    /// <summary>
    /// Gets original fractional order quantity (with the fractional part).
    /// </summary>
    Decimal? Quantity { get; }

    /// <summary>
    /// Gets filled order quantity (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal FilledQuantity { get; }

    /// <summary>
    /// Gets original order quantity (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64 IntegerQuantity { get; }

    /// <summary>
    /// Gets filled order quantity (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64 IntegerFilledQuantity { get; }

    /// <summary>
    /// Gets order type.
    /// </summary>
    [UsedImplicitly]
    OrderType OrderType { get; }

    /// <summary>
    /// Gets order class.
    /// </summary>
    [UsedImplicitly]
    OrderClass OrderClass { get; }

    /// <summary>
    /// Gets order side (buy or sell).
    /// </summary>
    [UsedImplicitly]
    OrderSide OrderSide { get; }

    /// <summary>
    /// Gets order duration.
    /// </summary>
    [UsedImplicitly]
    TimeInForce TimeInForce { get; }

    /// <summary>
    /// Gets order limit price for limit and stop-limit orders.
    /// </summary>
    [UsedImplicitly]
    Decimal? LimitPrice { get; }

    /// <summary>
    /// Gets order stop price for stop and stop-limit orders.
    /// </summary>
    [UsedImplicitly]
    Decimal? StopPrice { get; }

    /// <summary>
    /// Gets the profit taking limit price for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public Decimal? TrailOffsetInDollars { get; }

    /// <summary>
    /// Gets the stop loss stop price for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public Decimal? TrailOffsetInPercent { get; }

    /// <summary>
    /// Gets the current high water mark price for trailing stop orders.
    /// </summary>
    [UsedImplicitly]
    public Decimal? HighWaterMark { get; }

    /// <summary>
    /// Gets order average fill price.
    /// </summary>
    [UsedImplicitly]
    Decimal? AverageFillPrice { get; }

    /// <summary>
    /// Gets current order status.
    /// </summary>
    [UsedImplicitly]
    OrderStatus OrderStatus { get; }

    /// <summary>
    /// Gets the order ID that this order was replaced by.
    /// </summary>
    [UsedImplicitly]
    Guid? ReplacedByOrderId { get; }

    /// <summary>
    /// Gets the order ID that this order replaces.
    /// </summary>
    [UsedImplicitly]
    Guid? ReplacesOrderId { get; }

    /// <summary>
    /// Gets legs for this order.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IOrder> Legs { get; }
}
