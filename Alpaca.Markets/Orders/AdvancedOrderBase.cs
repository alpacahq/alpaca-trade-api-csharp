using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base data for advanced order types, never used directly by any code.
    /// </summary>
    public abstract class AdvancedOrderBase : OrderBase
    {
        internal AdvancedOrderBase(
            OrderBase baseOrder,
            OrderClass orderClass)
            : base(baseOrder)
        {
            BaseOrder = baseOrder;
            OrderClass = orderClass;
        }

        /// <summary>
        /// Gets or sets the order class for advanced order types.
        /// </summary>
        [UsedImplicitly] 
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
