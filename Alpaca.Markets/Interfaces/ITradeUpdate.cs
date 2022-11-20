namespace Alpaca.Markets;

/// <summary>
/// Encapsulates trade update information from Alpaca streaming API.
/// </summary>
[CLSCompliant(false)]
public interface ITradeUpdate
{
    /// <summary>
    /// Gets trade update reason.
    /// </summary>
    [UsedImplicitly]
    [SuppressMessage(
        "Naming", "CA1716:Identifiers should not match keywords",
        Justification = "Already used by clients and creates conflict only in VB.NET")]
    TradeEvent Event { get; }

    /// <summary>
    /// Gets optional order execution identifier.
    /// </summary>
    [UsedImplicitly]
    Guid? ExecutionId { get; }

    /// <summary>
    /// Gets updated trade price level.
    /// </summary>
    [UsedImplicitly]
    Decimal? Price { get; }

    /// <summary>
    /// Gets updated position quantity (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal? PositionQuantity { get; }

    /// <summary>
    /// Gets updated position quantity (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64? PositionIntegerQuantity { get; }

    /// <summary>
    /// Gets updated trade quantity (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal? TradeQuantity { get; }

    /// <summary>
    /// Gets updated trade quantity (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64? TradeIntegerQuantity { get; }

    /// <summary>
    /// Gets update timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? TimestampUtc { get; }

    /// <summary>
    /// Gets related order object.
    /// </summary>
    [UsedImplicitly]
    IOrder Order { get; }
}
