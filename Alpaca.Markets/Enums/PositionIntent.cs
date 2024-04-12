namespace Alpaca.Markets;

/// <summary>
/// Position intent for order placement in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum PositionIntent
{
    /// <summary>
    /// Buy to open a long position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "buy_to_open")]
    BuyToOpen,

    /// <summary>
    /// Buy to close a short position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "buy_to_close")]
    BuyToClose,

    /// <summary>
    /// Sell to open a short position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "sell_to_open")]
    SellToOpen,

    /// <summary>
    /// Sell to close a long position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "sell_to_close")]
    SellToClose
}
