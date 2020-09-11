using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Set of extensions methods for implementing the fluent interface for the <see cref="OrderBase"/> inheritors.
    /// </summary>
    public static class OrderBaseExtensions
    {
        /// <summary>
        /// Sets the new value for the <see cref="OrderBase.Duration"/> property of the target order.
        /// </summary>
        /// <param name="order">Target order for changing <see cref="OrderBase.Duration"/> property.</param>
        /// <param name="duration">The new <see cref="OrderBase.Duration"/> property value.</param>
        /// <typeparam name="TOrder">Type of target order for altering.</typeparam>
        /// <returns>Fluent interface - returns the <paramref name="order"/> object.</returns>
        public static TOrder WithDuration<TOrder>(
            this TOrder order,
            TimeInForce duration)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).Duration = duration;
            return order;
        }

        /// <summary>
        /// Sets the new value for the <see cref="OrderBase.ClientOrderId"/> property of the target order.
        /// </summary>
        /// <param name="order">Target order for changing <see cref="OrderBase.ClientOrderId"/> property.</param>
        /// <param name="clientOrderId">The new <see cref="OrderBase.ClientOrderId"/> property value.</param>
        /// <typeparam name="TOrder">Type of target order for altering.</typeparam>
        /// <returns>Fluent interface - returns the <paramref name="order"/> object.</returns>
        public static TOrder WithClientOrderId<TOrder>(
            this TOrder order,
            String clientOrderId)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).ClientOrderId = clientOrderId;
            return order;
        }

        /// <summary>
        /// Sets the new value for the <see cref="OrderBase.ExtendedHours"/> property of the target order.
        /// </summary>
        /// <param name="order">Target order for changing <see cref="OrderBase.ExtendedHours"/> property.</param>
        /// <param name="extendedHours">The new <see cref="OrderBase.ExtendedHours"/> property value.</param>
        /// <typeparam name="TOrder">Type of target order for altering.</typeparam>
        /// <returns>Fluent interface - returns the <paramref name="order"/> object.</returns>
        public static TOrder WithExtendedHours<TOrder>(
            this TOrder order,
            Boolean extendedHours)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).ExtendedHours = extendedHours;
            return order;
        }
    }
}
