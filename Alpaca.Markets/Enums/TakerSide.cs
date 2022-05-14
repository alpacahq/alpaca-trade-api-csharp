namespace Alpaca.Markets;

/// <summary>
/// Possible crypto taker side types for Alpaca Crypto Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TakerSide
{
    /// <summary>
    /// Unspecified crypto trade take side.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "-")]
    Unknown,

    /// <summary>
    /// Buy crypto trade take side.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "B")]
    Buy,

    /// <summary>
    /// Sell crypto trade take side.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "S")]
    Sell
}
