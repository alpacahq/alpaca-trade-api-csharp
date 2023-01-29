namespace Alpaca.Markets;

/// <summary>
/// Trade event in Alpaca trade update stream
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TradeEvent
{
    /// <summary>
    /// New working order.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "new")]
    New,

    /// <summary>
    /// Partial fill (event) on order.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "partial_fill")]
    PartialFill,

    /// <summary>
    /// Order completely filled.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "fill")]
    Fill,

    /// <summary>
    /// Order processing done.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "done_for_day")]
    DoneForDay,

    /// <summary>
    /// Order cancelled.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "canceled")]
    Canceled,

    /// <summary>
    /// Order cancellation request pending.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "pending_cancel")]
    PendingCancel,

    /// <summary>
    /// Order processing stopped by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stopped")]
    Stopped,

    /// <summary>
    /// Order rejected by server side.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "rejected")]
    Rejected,

    /// <summary>
    /// Order processing suspended by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "suspended")]
    Suspended,

    /// <summary>
    /// Initial new order request pending.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "pending_new")]
    PendingNew,

    /// <summary>
    /// Order information calculated by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "calculated")]
    Calculated,

    /// <summary>
    /// Order expired.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "expired")]
    Expired,

    /// <summary>
    /// Order cancellation was rejected.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "order_cancel_rejected")]
    OrderCancelRejected,

    /// <summary>
    /// Requested replacement of an order was processed.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "replaced")]
    Replaced,

    /// <summary>
    /// The order is awaiting replacement.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "pending_replace")]
    PendingReplace,

    /// <summary>
    /// The order replace has been rejected.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "order_replace_rejected")]
    ReplaceRejected,

    /// <summary>
    /// Order partially filled (status).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "partially_filled")]
    PartiallyFilled,

    /// <summary>
    /// Order accepted by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "accepted")]
    Accepted,

    /// <summary>
    /// Order held pending trigger event.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "held")]
    Held,

    /// <summary>
    /// Order review pending (order cost basis is large).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "pending_review")]
    PendingReview
}
