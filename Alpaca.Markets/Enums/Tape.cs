namespace Alpaca.Markets;

/// <summary>
/// Supported tape types for Alpaca Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum Tape
{
    /// <summary>
    /// Tape A - NYSE.
    /// </summary>
    [UsedImplicitly]
    A,

    /// <summary>
    /// Tape B - NYSE Arca and NYSE Amex.
    /// </summary>
    [UsedImplicitly]
    B,

    /// <summary>
    /// Tape C - NASDAQ.
    /// </summary>
    [UsedImplicitly]
    C
}
