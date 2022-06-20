namespace Alpaca.Markets;

/// <summary>
/// Set of extension methods for <see cref="IOrder"/> interface.
/// </summary>
public static class OrderExtensions
{
    /// <summary>
    /// Gets order quantity as instance of the <see cref="OrderQuantity"/> structure.
    /// </summary>
    /// <param name="order">Order for reading quantity value.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="order"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Both fractional and notional order quantity values are null.
    /// </exception>
    /// <returns>Fractional or notional order quantity value.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static OrderQuantity GetOrderQuantity(
        this IOrder order)
    {
        var notNullOrder = order.EnsureNotNull();
        return notNullOrder.Quantity.HasValue
            ? OrderQuantity.Fractional(notNullOrder.Quantity.Value)
            : notNullOrder.Notional.HasValue
                ? OrderQuantity.Notional(notNullOrder.Notional.Value)
                : throw new InvalidOperationException();
    }
}
