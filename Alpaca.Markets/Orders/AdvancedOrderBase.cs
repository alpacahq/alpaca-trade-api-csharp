namespace Alpaca.Markets
{
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
        protected internal AdvancedOrderBase(
            OrderBase baseOrder,
            OrderClass orderClass)
            : base(baseOrder.EnsureNotNull(nameof(baseOrder)))
        {
            BaseOrder = baseOrder;
            OrderClass = orderClass;
        }

        /// <summary>
        /// Gets or sets the order class for advanced order types.
        /// </summary>
        public OrderClass OrderClass { get; }

        internal override JsonNewOrder GetJsonRequest()
        {
            BaseOrder.ClientOrderId = ClientOrderId;
            BaseOrder.ExtendedHours = ExtendedHours;
            BaseOrder.Duration = Duration;

            return BaseOrder.GetJsonRequest()
                .WithOrderClass(OrderClass);
        }

        private OrderBase BaseOrder { get; }
    }
}
