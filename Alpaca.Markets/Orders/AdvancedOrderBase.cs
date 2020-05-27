namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base data for advanced order types, never used directly by any code.
    /// </summary>
    public abstract class AdvancedOrderBase : OrderBase
    {
        internal AdvancedOrderBase(
            OrderBase order,
            OrderClass orderClass)
            : base(order) =>
            OrderClass = orderClass;

        /// <summary>
        /// Gets or sets the order class for advanced order types.
        /// </summary>
        public OrderClass OrderClass { get; }

        internal override JsonNewOrder GetJsonRequest() =>
            base.GetJsonRequest()
                .WithOrderClass(OrderClass);
    }
}
