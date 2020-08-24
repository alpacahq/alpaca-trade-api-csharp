﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
        Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
    internal sealed class JsonOrder : IOrder
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public Guid OrderId { get; set; }

        [JsonProperty(PropertyName = "client_order_id", Required = Required.Always)]
        public String? ClientOrderId { get; set; }

        [JsonProperty(PropertyName = "created_at", Required = Required.Default)]
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAtUtc => CreatedAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "updated_at", Required = Required.Default)]
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public DateTime? UpdatedAtUtc => UpdatedAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "submitted_at", Required = Required.Default)]
        public DateTime? SubmittedAt { get; set; }

        [JsonIgnore]
        public DateTime? SubmittedAtUtc => SubmittedAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "filled_at", Required = Required.Default)]
        public DateTime? FilledAt { get; set; }

        [JsonIgnore]
        public DateTime? FilledAtUtc => FilledAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "expired_at ", Required = Required.Default)]
        public DateTime? ExpiredAt { get; set; }

        [JsonIgnore]
        public DateTime? ExpiredAtUtc => ExpiredAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "canceled_at", Required = Required.Default)]
        public DateTime? CancelledAt { get; set; }

        [JsonIgnore]
        public DateTime? CancelledAtUtc => CancelledAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "failed_at", Required = Required.Default)]
        public DateTime? FailedAt { get; set; }

        [JsonIgnore]
        public DateTime? FailedAtUtc => FailedAt.AsUtcDateTime();

        [JsonProperty(PropertyName = "asset_id", Required = Required.Always)]
        public Guid AssetId { get; set; }

        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Symbol { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "asset_class", Required = Required.Always)]
        public AssetClass AssetClass { get; set; }

        [JsonProperty(PropertyName = "qty", Required = Required.Always)]
        public Int64 Quantity { get; set; }

        [JsonProperty(PropertyName = "filled_qty", Required = Required.Always)]
        public Int64 FilledQuantity { get; set; }

        [JsonProperty(PropertyName = "order_type", Required = Required.Always)]
        public OrderType OrderType { get; set; }

        [JsonProperty(PropertyName = "side", Required = Required.Always)]
        public OrderSide OrderSide { get; set; }

        [JsonProperty(PropertyName = "time_in_force", Required = Required.Always)]
        public TimeInForce TimeInForce { get; set; }

        [JsonProperty(PropertyName = "limit_price", Required = Required.Default)]
        public Decimal? LimitPrice { get; set; }

        [JsonProperty(PropertyName = "stop_price", Required = Required.Default)]
        public Decimal? StopPrice { get; set; }

        [JsonProperty(PropertyName = "trail_price", Required = Required.Default)]
        public Decimal? TrailOffsetInDollars { get; set; }

        [JsonProperty(PropertyName = "trail_percent", Required = Required.Default)]
        public Decimal? TrailOffsetInPercent { get; set; }

        [JsonProperty(PropertyName = "hwm", Required = Required.Default)]
        public Decimal? HighWaterMark { get; set; }
        
        [JsonProperty(PropertyName = "filled_avg_price", Required = Required.Default)]
        public Decimal? AverageFillPrice { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public OrderStatus OrderStatus { get; set; }

        [JsonProperty(PropertyName = "legs", Required = Required.Default)]
        public List<JsonOrder>? LegsList { get; set; }

        [JsonIgnore]
        public IReadOnlyList<IOrder> Legs => LegsList.EmptyIfNull<IOrder, JsonOrder>();
    }
}
