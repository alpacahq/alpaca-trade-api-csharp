namespace Alpaca.Markets;

/// <summary>
/// Encapsulates trade correction information from Alpaca APIs.
/// </summary>
[CLSCompliant(false)]
public interface ICorrection
{
    /// <summary>
    /// Gets information about the original trade.
    /// </summary>
    [UsedImplicitly]
    ITrade OriginalTrade { get; }

    /// <summary>
    /// Gets information about the corrected trade.
    /// </summary>
    [UsedImplicitly]
    ITrade CorrectedTrade { get; }
}
