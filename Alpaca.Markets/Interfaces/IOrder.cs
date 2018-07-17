using System;

namespace Alpaca.Markets
{
    public interface IOrder
    {
        Guid OrderId { get; }

        String ClientOrderId { get; }

        DateTime? CreatedAt { get; }

        DateTime? UpdatedAt { get; }

        DateTime? SubmittedAt { get; }

        DateTime? FilledAt { get; }

        DateTime? ExpiredAt { get; }

        DateTime? CancelledAt { get; }

        DateTime? FailedAt { get; }

        Guid AssetId { get; }

        String Symbol { get; }

        AssetClass AssetClass { get; }

        Int64 Quantity { get; }

        Int64 FilledQuantity { get; }

        OrderType OrderType { get; }

        OrderSide OrderSide { get; }

        TimeInForce TimeInForce { get; }

        Decimal? LimitPrice { get; }

        Decimal? StopPrice { get; }

        Decimal? AverageFillPrice { get; }

        OrderStatus OrderStatus { get; }
    }
}