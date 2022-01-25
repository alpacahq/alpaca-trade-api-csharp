namespace Alpaca.Markets;

/// <summary>
/// Encapsulates single trading day information from Alpaca REST API.
/// </summary>
public interface IIntervalCalendar
{
    /// <summary>
    /// Gets trading open and close times in EST time zone.
    /// </summary>
    public OpenClose Trading { get; }

    /// <summary>
    /// Gets session open and close times in EST time zone.
    /// </summary>
    public OpenClose Session { get; }
}
