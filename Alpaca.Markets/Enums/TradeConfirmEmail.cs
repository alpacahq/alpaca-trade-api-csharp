namespace Alpaca.Markets;

/// <summary>
/// Notification level for order fill emails.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TradeConfirmEmail
{
    /// <summary>
    /// Never send email notification for order fills.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Send email notification for all order fills.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "all")]
    All
}
