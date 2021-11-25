namespace Alpaca.Markets;

/// <summary>
/// Encapsulates access point for setting time interval filtering on request instance.
/// </summary>
public interface IRequestWithDateInterval
{
    /// <summary>
    /// Sets time interval value for the current request instance.
    /// </summary>
    void SetInterval(IDateInterval value);
}
