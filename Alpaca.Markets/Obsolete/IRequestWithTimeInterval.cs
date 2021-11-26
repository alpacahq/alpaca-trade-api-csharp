namespace Alpaca.Markets;

/// <summary>
/// Encapsulates access point for setting time interval filtering on request instance.
/// </summary>
/// <typeparam name="TInterval">The sort of time interval (inclusive or exclusive).</typeparam>
[Obsolete("Use WithInterval methods of the requests parameters directly.", false)]
public interface IRequestWithTimeInterval<in TInterval>
    where TInterval : ITimeInterval
{
    /// <summary>
    /// Sets time interval value for the current request instance.
    /// </summary>
    void SetInterval(TInterval value);
}