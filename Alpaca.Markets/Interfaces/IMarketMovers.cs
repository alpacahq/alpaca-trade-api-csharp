namespace Alpaca.Markets;

/// <summary>
/// Encapsulates two lists of market movers from Alpaca APIs.
/// </summary>
public interface IMarketMovers
{
    /// <summary>
    /// Gets list of the top market losers.
    /// </summary>
    IReadOnlyList<IMarketMover> Losers { get; }

    /// <summary>
    /// Gets list of the top market gainers.
    /// </summary>
    IReadOnlyList<IMarketMover> Gainers { get; }
}
