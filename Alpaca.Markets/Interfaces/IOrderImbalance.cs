namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic order imbalance information from Alpaca APIs.
/// </summary>
public interface IOrderImbalance
{
    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the order imbalance update timestamp in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets the reference price for the imbalance. Typically NYSE Last Sale,
    /// or bid/offer if last sale is outside spread.
    /// </summary>
    [UsedImplicitly]
    Decimal ReferencePrice { get; }

    /// <summary>
    /// Gets the volume of shares that can be matched at the reference price.
    /// </summary>
    [UsedImplicitly]
    Int64 PairedShares { get; }

    /// <summary>
    /// Gets the volume of shares that cannot be paired (buy or sell excess).
    /// </summary>
    [UsedImplicitly]
    Int64 ImbalanceShares { get; }

    /// <summary>
    /// Gets the side of the imbalance: "B" for buy side, "S" for sell side.
    /// </summary>
    [UsedImplicitly]
    String ImbalanceSide { get; }

    /// <summary>
    /// Gets tape.
    /// </summary>
    [UsedImplicitly]
    String Tape { get; }

    /// <summary>
    /// Gets the current auction type. "O" for opening auction, "C" for closing auction.
    /// </summary>
    [UsedImplicitly]
    String AuctionType { get; }
}