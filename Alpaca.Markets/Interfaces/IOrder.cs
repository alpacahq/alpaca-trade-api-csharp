﻿using System;
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
        DateTime? CreatedAt { get; }

        /// <summary>
        /// Gets last order update timestamp.
        /// </summary>
        DateTime? UpdatedAt { get; }

        /// <summary>
        /// Gets order submission timestamp.
        /// </summary>
        DateTime? SubmittedAt { get; }

        /// <summary>
        /// Gets order fill timestamp.
        /// </summary>
        DateTime? FilledAt { get; }

        /// <summary>
        /// Gets order expiration timestamp.
        /// </summary>
        DateTime? ExpiredAt { get; }

        /// <summary>
        /// Gets order cancellation timestamp.
        /// </summary>
        DateTime? CancelledAt { get; }

        /// <summary>
        /// Gets order rejection timestamp.
        /// </summary>
        DateTime? FailedAt { get; }

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
