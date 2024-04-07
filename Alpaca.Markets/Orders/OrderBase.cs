namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base data for any order types, never used directly by any code.
/// </summary>
public abstract class OrderBase : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of the <see cref="OrderBase"/> class.
    /// </summary>
    /// <param name="symbol">Alpaca symbol for order.</param>
    /// <param name="quantity">Order quantity (absolute value).</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="type">Order type (market, limit, stop, stop-limit).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    protected internal OrderBase(
        String symbol,
        OrderQuantity quantity,
        OrderSide side,
        OrderType type)
    {
        Symbol = symbol.EnsureNotNull();
        Quantity = quantity;
        Side = side;
        Type = type;
    }

    /// <summary>
    /// Creates new instance of the <see cref="OrderBase"/> class.
    /// </summary>
    /// <param name="baseOrder">Base order for getting parameters.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="baseOrder"/> argument is <c>null</c>.
    /// </exception>
    protected internal OrderBase(
        OrderBase baseOrder)
        : this(
            baseOrder.EnsureNotNull().Symbol,
            baseOrder.Quantity,
            baseOrder.Side,
            baseOrder.Type)
    {
        Duration = baseOrder.Duration;
        ClientOrderId = baseOrder.ClientOrderId;
        ExtendedHours = baseOrder.ExtendedHours;
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
    public TimeInForce Duration { get; set; } = TimeInForce.Day;

    /// <summary>
    /// Gets or sets the client order ID. This is a user-specified ID that must be unique if provided.
    /// </summary>
    public String? ClientOrderId { get; set; }

    /// <summary>
    /// Gets or sets flag indicating that order should be allowed to execute during extended hours trading.
    /// </summary>
    public Boolean? ExtendedHours { get; set; }
    
    /// <summary>
    /// Gets or sets the optional position intent for order placement.
    /// </summary>
    [UsedImplicitly]
    public PositionIntent? PositionIntent { get; set; }

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        ClientOrderId = ClientOrderId?.TrimClientOrderId();
        yield return Symbol.TryValidateSymbolName();
        yield return TryValidateQuantity();
    }

    // ReSharper disable once MemberCanBeProtected.Global
    internal virtual RequestValidationException? TryValidateQuantity() =>
        Quantity.TryValidateQuantity();

    internal virtual JsonNewOrder GetJsonRequest() =>
        new()
        {
            Symbol = Symbol,
            OrderSide = Side,
            OrderType = Type,
            TimeInForce = Duration,
            ExtendedHours = ExtendedHours,
            ClientOrderId = ClientOrderId,
            PositionIntent = PositionIntent,
            Notional = Quantity.AsNotional(),
            Quantity = Quantity.AsFractional()
        };
}
