using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrderBaseExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="duration"></param>
        /// <typeparam name="TOrder"></typeparam>
        /// <returns></returns>
        public static TOrder WithDuration<TOrder>(
            TOrder order,
            TimeInForce duration)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).Duration = duration;
            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="clientOrderId"></param>
        /// <typeparam name="TOrder"></typeparam>
        /// <returns></returns>
        public static TOrder WithClientOrderId<TOrder>(
            TOrder order,
            String clientOrderId)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).ClientOrderId = clientOrderId;
            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="extendedHours"></param>
        /// <typeparam name="TOrder"></typeparam>
        /// <returns></returns>
        public static TOrder WithExtendedHours<TOrder>(
            TOrder order,
            Boolean extendedHours)
            where TOrder : OrderBase
        {
            order.EnsureNotNull(nameof(order)).ExtendedHours = extendedHours;
            return order;
        }
    }
}
