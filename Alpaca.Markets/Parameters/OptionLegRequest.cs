namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.PostOrderAsync(NewOrderRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class OptionLegRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="OptionLegRequest"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <param name="positionIntent">Order position intent.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OptionLegRequest(
        String symbol,
        Decimal ratioQuantity,
        OrderSide side,
        PositionIntent positionIntent)
    {
        Symbol = symbol.EnsureNotNull();
        PositionIntent = positionIntent;
        RatioQuantity = ratioQuantity;
        Side = side;
    }

    /// <summary>
    /// Creates new instance of <see cref="OptionLegRequest"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="side">Order side (buy or sell).</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OptionLegRequest(
        String symbol,
        Decimal ratioQuantity,
        OrderSide side)
    {
        Symbol = symbol.EnsureNotNull();
        RatioQuantity = ratioQuantity;
        Side = side;
    }

    /// <summary>
    /// Creates new instance of <see cref="OptionLegRequest"/> object.
    /// </summary>
    /// <param name="symbol">Order asset symbol.</param>
    /// <param name="ratioQuantity">Order quantity.</param>
    /// <param name="positionIntent">Order position intent.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public OptionLegRequest(
        String symbol,
        Decimal ratioQuantity,
        PositionIntent positionIntent)
    {
        Symbol = symbol.EnsureNotNull();
        PositionIntent = positionIntent;
        RatioQuantity = ratioQuantity;
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

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }

    internal JsonOrderLeg GetJsonRequest() =>
        new()
        {
            Symbol = Symbol,
            OrderSide = Side,
            RatioQuantity = RatioQuantity,
            PositionIntent = PositionIntent
        };
}
