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
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="quantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="type">Order type.</param>
    /// <param name="duration">Order duration.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
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
    /// Gets the new order asset symbol.
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
    /// Gets or sets the client order ID. This user-specified ID must be unique if set.
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

    /// <summary>
    /// Gets or sets the optional position intent for order placement.
    /// </summary>
    [UsedImplicitly]
    public PositionIntent? PositionIntent { get; set; }

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        ClientOrderId = ClientOrderId?.TrimClientOrderId();
        yield return Symbol.TryValidateSymbolName();
        yield return Quantity.TryValidateQuantity();
    }
    internal JsonNewOrder GetJsonRequest() =>
        new()
        {
            Symbol = Symbol,
            OrderSide = Side,
            OrderType = Type,
            StopPrice = StopPrice,
            TimeInForce = Duration,
            LimitPrice = LimitPrice,
            OrderClass = OrderClass,
            ClientOrderId = ClientOrderId,
            ExtendedHours = ExtendedHours,
            PositionIntent = PositionIntent,
            Notional = Quantity.AsNotional(),
            Quantity = Quantity.AsFractional(),
            TrailOffsetInDollars = TrailOffsetInDollars,
            TrailOffsetInPercent = TrailOffsetInPercent,
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
        };
}
