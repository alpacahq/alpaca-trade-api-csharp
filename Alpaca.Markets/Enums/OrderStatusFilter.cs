namespace Alpaca.Markets;

/// <summary>
/// Order statuses filter for <see cref="IAlpacaTradingClient.ListOrdersAsync"/> call from Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OrderStatusFilter
{
    /// <summary>
    /// Returns only open orders.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "open")]
    Open,

    /// <summary>
    /// Returns only closed orders.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "closed")]
    Closed,

    /// <summary>
    /// Returns all orders.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "all")]
    All
}
