namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base data for advanced order types, never used directly by any code.
/// </summary>
public abstract class AdvancedOrderBase : OrderBase
{
    /// <summary>
    /// Creates new instance of the <see cref="AdvancedOrderBase"/> class.
    /// </summary>
    /// <param name="baseOrder">Base order object for creating advanced one.</param>
    /// <param name="orderClass">Advanced order class for new smart order.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="baseOrder"/> argument is <c>null</c>.
    /// </exception>
    protected internal AdvancedOrderBase(
        SimpleOrderBase baseOrder,
        OrderClass orderClass)
        : base(baseOrder.EnsureNotNull())
    {
        BaseOrder = baseOrder;
        OrderClass = orderClass;
    }

    /// <summary>
    /// Gets or sets the order class for advanced order types.
    /// </summary>
    [UsedImplicitly]
    public OrderClass OrderClass { get; }
    
    internal override RequestValidationException? TryValidateQuantity() =>
        BaseOrder.TryValidateQuantity();

    internal override JsonNewOrder GetJsonRequest()
    {
        BaseOrder.PositionIntent = PositionIntent;
        BaseOrder.ClientOrderId = ClientOrderId;
        BaseOrder.ExtendedHours = ExtendedHours;
        BaseOrder.Duration = Duration;

        return BaseOrder.GetJsonRequest()
            .WithOrderClass(OrderClass);
    }

    internal SimpleOrderBase BaseOrder { get; }
}
