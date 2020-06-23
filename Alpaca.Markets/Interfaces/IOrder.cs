using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates order information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IOrder
    {
        /// <summary>
        /// Gets unique server-side order identifier.
        /// </summary>
        Guid OrderId { get; }

        /// <summary>
        /// Gets client-side order identifier.
        /// </summary>
        String? ClientOrderId { get; }

        /// <summary>
        /// Gets order creation timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the CreatedAtUtc property instead.", false)]
        DateTime? CreatedAt { get; }

        /// <summary>
        /// Gets order creation timestamp in UTC time zone.
        /// </summary>
        DateTime? CreatedAtUtc { get; }

        /// <summary>
        /// Gets last order update timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the UpdatedAtUtc property instead.", false)]
        DateTime? UpdatedAt { get; }

        /// <summary>
        /// Gets last order update timestamp in UTC time zone.
        /// </summary>
        DateTime? UpdatedAtUtc { get; }

        /// <summary>
        /// Gets order submission timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the SubmittedAtUtc property instead.", false)]
        DateTime? SubmittedAt { get; }

        /// <summary>
        /// Gets order submission timestamp in UTC time zone.
        /// </summary>
        DateTime? SubmittedAtUtc { get; }

        /// <summary>
        /// Gets order fill timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the FilledAtUtc property instead.", false)]
        DateTime? FilledAt { get; }

        /// <summary>
        /// Gets order fill timestamp in UTC time zone.
        /// </summary>
        DateTime? FilledAtUtc { get; }

        /// <summary>
        /// Gets order expiration timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the ExpiredAtUtc property instead.", false)]
        DateTime? ExpiredAt { get; }

        /// <summary>
        /// Gets order expiration timestamp in UTC time zone.
        /// </summary>
        DateTime? ExpiredAtUtc { get; }

        /// <summary>
        /// Gets order cancellation timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the CancelledAtUtc property instead.", false)]
        DateTime? CancelledAt { get; }

        /// <summary>
        /// Gets order cancellation timestamp in UTC time zone.
        /// </summary>
        DateTime? CancelledAtUtc { get; }

        /// <summary>
        /// Gets order rejection timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the FailedAtUtc property instead.", false)]
        DateTime? FailedAt { get; }

        /// <summary>
        /// Gets order rejection timestamp in UTC time zone.
        /// </summary>
        DateTime? FailedAtUtc { get; }

        /// <summary>
        /// Gets unique asset identifier.
        /// </summary>
        Guid AssetId { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets asset class.
        /// </summary>
        AssetClass AssetClass { get; }

        /// <summary>
        /// Gets original order quantity.
        /// </summary>
        Int64 Quantity { get; }

        /// <summary>
        /// Gets filled order quantity.
        /// </summary>
        Int64 FilledQuantity { get; }

        /// <summary>
        /// Gets order type.
        /// </summary>
        OrderType OrderType { get; }

        /// <summary>
        /// Gets order side (buy or sell).
        /// </summary>
        OrderSide OrderSide { get; }

        /// <summary>
        /// Gets order duration.
        /// </summary>
        TimeInForce TimeInForce { get; }

        /// <summary>
        /// Gets order limit price for limit and stop-limit orders.
        /// </summary>
        Decimal? LimitPrice { get; }

        /// <summary>
        /// Gets order stop price for stop and stop-limit orders.
        /// </summary>
        Decimal? StopPrice { get; }

        /// <summary>
        /// Gets order average fill price.
        /// </summary>
        Decimal? AverageFillPrice { get; }

        /// <summary>
        /// Gets current order status.
        /// </summary>
        OrderStatus OrderStatus { get; }

        /// <summary>
        /// Gets legs for this order.
        /// </summary>
        IReadOnlyList<IOrder>? Legs { get; }
    }
}
