namespace Alpaca.Markets;

/// <summary>
/// Order side in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(OrderSideEnumConverter))]
public enum OrderSide
{
    /// <summary>
    /// Buy order.
    /// </summary>
    [EnumMember(Value = "buy")]
    Buy,

    /// <summary>
    /// Sell order.
    /// </summary>
    [EnumMember(Value = "sell")]
    Sell
}
