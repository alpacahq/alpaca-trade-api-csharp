namespace Alpaca.Markets;

/// <summary>
/// Encapsulates snapshot information from the Alpaca REST API.
/// </summary>
[CLSCompliant(false)]
public interface ISnapshot
{
    /// <summary>
    /// Gets the snapshot's asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the latest quote information.
    /// </summary>
    [UsedImplicitly]
    IQuote? Quote { get; }

    /// <summary>
    /// Gets the latest trade information.
    /// </summary>
    [UsedImplicitly]
    ITrade? Trade { get; }

    /// <summary>
    /// Gets the current minute bar information.
    /// </summary>
    [UsedImplicitly]
    IBar? MinuteBar { get; }

    /// <summary>
    /// Gets the current daily bar information.
    /// </summary>
    [UsedImplicitly]
    IBar? CurrentDailyBar { get; }

    /// <summary>
    /// Gets the previous daily bar information.
    /// </summary>
    [UsedImplicitly]
    IBar? PreviousDailyBar { get; }
}
