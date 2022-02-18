namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.PostOrderAsync(NewOrderRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class NewOrderRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="NewOrderRequest"/> object.
    /// </summary>
    /// <param name="symbol">Order asset name.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="type">Order type.</param>
    /// <param name="duration">Order duration.</param>
    public NewOrderRequest(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        OrderType type,
        TimeInForce duration)
    {
        Symbol = symbol.EnsureNotNull();
        Quantity = quantity;
        Side = side;
        Type = type;
        Duration = duration;
    }

    /// <summary>
    /// Gets the new order asset name.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    /// <summary>
    /// Gets the new order quantity.
    /// </summary>
    [UsedImplicitly]
    public OrderQuantity Quantity { get; }

    /// <summary>
    /// Gets the new order side (buy or sell).
    /// </summary>
    [UsedImplicitly]
    public OrderSide Side { get; }

    /// <summary>
    /// Gets the new order type.
    /// </summary>
    [UsedImplicitly]
    public OrderType Type { get; }

    /// <summary>
    /// Gets the new order duration.
    /// </summary>
    [UsedImplicitly]
    public TimeInForce Duration { get; }

    /// <summary>
    /// Gets or sets the new order limit price.
    /// </summary>
    [UsedImplicitly]
    public Decimal? LimitPrice { get; set; }

    /// <summary>
    /// Gets or sets the new order stop price.
    /// </summary>
    [UsedImplicitly]
    public Decimal? StopPrice { get; set; }

    /// <summary>
    /// Gets or sets the new trailing order trail price offset in dollars.
    /// </summary>
    [UsedImplicitly]
    public Decimal? TrailOffsetInDollars { get; set; }

    /// <summary>
    /// Gets or sets the new trailing order trail price offset in percent.
    /// </summary>
    [UsedImplicitly]
    public Decimal? TrailOffsetInPercent { get; set; }

    /// <summary>
    /// Gets or sets the client order ID.
    /// </summary>
    [UsedImplicitly]
    public String? ClientOrderId { get; set; }

    /// <summary>
    /// Gets or sets flag indicating that order should be allowed to execute during extended hours trading.
    /// </summary>
    [UsedImplicitly]
    public Boolean? ExtendedHours { get; set; }

    /// <summary>
    /// Gets or sets the order class for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public OrderClass? OrderClass { get; set; }

    /// <summary>
    /// Gets or sets the profit taking limit price for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public Decimal? TakeProfitLimitPrice { get; set; }

    /// <summary>
    /// Gets or sets the stop loss stop price for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public Decimal? StopLossStopPrice { get; set; }

    /// <summary>
    /// Gets or sets the stop loss limit price for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public Decimal? StopLossLimitPrice { get; set; }

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        ClientOrderId = ClientOrderId?.TrimClientOrderId();
        yield return Symbol.TryValidateSymbolName();
        yield return Quantity.TryValidateQuantity();
    }

    internal JsonNewOrder GetJsonRequest() =>
        new JsonNewOrder
        {
            Symbol = Symbol,
            OrderSide = Side,
            OrderType = Type,
            TimeInForce = Duration,
            LimitPrice = LimitPrice,
            StopPrice = StopPrice,
            TrailOffsetInDollars = TrailOffsetInDollars,
            TrailOffsetInPercent = TrailOffsetInPercent,
            ClientOrderId = ClientOrderId,
            ExtendedHours = ExtendedHours,
            OrderClass = OrderClass,
            TakeProfit = TakeProfitLimitPrice is not null
                ? new JsonNewOrderAdvancedAttributes
                {
                    LimitPrice = TakeProfitLimitPrice
                }
                : null,
            StopLoss = StopLossStopPrice is not null ||
                       StopLossLimitPrice is not null
                ? new JsonNewOrderAdvancedAttributes
                {
                    StopPrice = StopLossStopPrice,
                    LimitPrice = StopLossLimitPrice
                }
                : null
        }.WithQuantity(Quantity);
}
