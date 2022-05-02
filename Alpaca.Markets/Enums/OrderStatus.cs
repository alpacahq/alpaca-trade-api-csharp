namespace Alpaca.Markets;

/// <summary>
/// Order status in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OrderStatus
{
    /// <summary>
    /// Order accepted by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "accepted")]
    Accepted,

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
    /// Order partially filled (status).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "partially_filled")]
    PartiallyFilled,

    /// <summary>
    /// Order completely filled.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "filled")]
    Filled,

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
    /// Order replaced (modified).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "replaced")]
    Replaced,

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
    /// Order rejected by server.
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
    /// Order accepted for bidding by server.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "accepted_for_bidding")]
    AcceptedForBidding,

    /// <summary>
    /// Order replacement request pending.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "pending_replace")]
    PendingReplace,

    /// <summary>
    /// Order completely filled.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "fill")]
    Fill,

    /// <summary>
    /// Order held pending trigger event.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "held")]
    Held
}
