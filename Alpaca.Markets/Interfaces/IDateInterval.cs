namespace Alpaca.Markets;

/// <summary>
/// Encapsulates time interval (from and till date/time points) for filtering requires.
/// </summary>
public interface IDateInterval
{
    /// <summary>
    /// Gets the starting date/time point of filtering interval.
    /// </summary>
    DateOnly? From { get; }

    /// <summary>
    /// Gets the ending date/time point of filtering interval.
    /// </summary>
    DateOnly? Into { get; }
}
