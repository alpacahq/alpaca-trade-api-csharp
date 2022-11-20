namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the basic auction information from Alpaca APIs.
/// </summary>
public interface IAuction
{
    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets auction date in UTC.
    /// </summary>
    [UsedImplicitly]
    [SuppressMessage("Naming", "CA1716:Identifiers should not match keywords",
        Justification = "This can cause problem only in VB.NET source code.")]
    DateOnly Date { get; }

    /// <summary>
    /// Gets daily auction openings.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IAuctionEntry> Openings { get; }

    /// <summary>
    /// Gets daily auction closings.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IAuctionEntry> Closings { get; }
}
