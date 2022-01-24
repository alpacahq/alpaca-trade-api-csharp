namespace Alpaca.Markets;

/// <summary>
/// Encapsulates single trading day information from Alpaca REST API.
/// </summary>
public interface IIntervalCalendar
{
    /// <summary>
    /// Gets trading open and close time in UTC time zone.
    /// </summary>
    public Interval<DateTime> TradingOpenCloseUtc { get; }

    /// <summary>
    /// Gets session open and close time in UTC time zone.
    /// </summary>
    public Interval<DateTime> SessionOpenCloseUtc { get; }

    /// <summary>
    /// Gets trading open and close time in EST time zone.
    /// </summary>
    public Interval<DateTimeOffset> TradingOpenCloseEst { get; }

    /// <summary>
    /// Gets session open and close time in EST time zone.
    /// </summary>
    public Interval<DateTimeOffset> SessionOpenCloseEst { get; }
}
