namespace Alpaca.Markets;

/// <summary>
/// Crypto currency exchanges supported by Alpaca REST API.
/// </summary>
[JsonConverter(typeof(CryptoExchangeEnumConverter))]
[SuppressMessage("ReSharper", "IdentifierTypo")]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public enum CryptoExchange
{
    /// <summary>
    /// Unknown exchange (i.e. one not supported by this version of SDK).
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "UNKNOWN")]
    Unknown,

    /// <summary>
    /// ErisX Exchange.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ERSX")]
    Ersx,

    /// <summary>
    /// FTX US Exchange.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "FTXU")]
    Ftx,

    /// <summary>
    /// Coinbase Exchange.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "CBSE")]
    Cbse
}
