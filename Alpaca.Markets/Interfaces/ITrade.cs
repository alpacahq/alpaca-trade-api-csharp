namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic trade information from Alpaca APIs.
/// </summary>
[CLSCompliant(false)]
public interface ITrade
{
    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets trade timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets trade price level.
    /// </summary>
    [UsedImplicitly]
    Decimal Price { get; }

    /// <summary>
    /// Gets trade quantity.
    /// </summary>
    [UsedImplicitly]
    Decimal Size { get; }

    /// <summary>
    /// Gets trade identifier.
    /// </summary>
    [UsedImplicitly]
    UInt64 TradeId { get; }

    /// <summary>
    /// Gets trade source exchange identifier.
    /// </summary>
    [UsedImplicitly]
    String Exchange { get; }

    /// <summary>
    /// Gets tape where trade occurred.
    /// </summary>
    [UsedImplicitly]
    String Tape { get; }

    /// <summary>
    /// Gets trade update reason if any.
    /// </summary>
    [UsedImplicitly]
    String Update { get; }

    /// <summary>
    /// Gets trade conditions list.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<String> Conditions { get; }

    /// <summary>
    /// Gets crypto trade taker side.
    /// </summary>
    [UsedImplicitly]
    TakerSide TakerSide { get; }
}
