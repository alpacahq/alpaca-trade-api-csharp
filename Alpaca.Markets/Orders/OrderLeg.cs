namespace Alpaca.Markets;

/// <summary>
/// Represents the single leg of the option multi-leg order.
/// </summary>
public sealed record OrderLeg
{
    /// <summary>
    /// Creates a new instance of the <see cref="OrderLeg"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OrderLeg(
        String symbol,
        Decimal ratioQuantity,
        OrderSide side)
        : this(symbol, ratioQuantity, side, null)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderLeg"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="positionIntent">Order position intent.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OrderLeg(
        String symbol,
        Decimal ratioQuantity,
        PositionIntent positionIntent)
        : this(symbol, ratioQuantity, null, positionIntent)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderLeg"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="positionIntent">Order position intent.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OrderLeg(
        String symbol,
        Decimal ratioQuantity,
        PositionIntent positionIntent,
        OrderSide side)
        : this(symbol, ratioQuantity, side, positionIntent)
    {
    }

    private OrderLeg(
        String symbol,
        Decimal ratioQuantity,
        OrderSide? side,
        PositionIntent? positionIntent)
    {
        Symbol = symbol.EnsureNotNull();
        RatioQuantity = ratioQuantity;
        PositionIntent = positionIntent;
        Side = side;
    }

    /// <summary>
    /// Gets the new order asset symbol.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    /// <summary>
    /// Gets the proportional quantity of this leg in relation to the overall multi-leg order quantity.
    /// </summary>
    [UsedImplicitly]
    public Decimal RatioQuantity { get; }

    /// <summary>
    /// Gets the new order side (buy or sell).
    /// </summary>
    [UsedImplicitly]
    public OrderSide? Side { get; }

    /// <summary>
    /// Gets the optional position intent for order placement.
    /// </summary>
    [UsedImplicitly]
    public PositionIntent? PositionIntent { get; }

    internal JsonOrderLeg GetJsonRequest() =>
        new ()
        {
            Symbol = Symbol,
            OrderSide = Side,
            RatioQuantity = RatioQuantity,
            PositionIntent = PositionIntent
        };
}
