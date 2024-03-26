namespace Alpaca.Markets;

/// <summary>
/// Encapsulates session starting and ending points - session open and close time in EST time zone.
/// </summary>
public readonly record struct OpenClose
{
    /// <summary>
    /// Creates new instance of the <see cref="OpenClose"/> structure.
    /// </summary>
    public OpenClose()
    {
        OpenEst = default;
        CloseEst = default;
    }

    /// <summary>
    /// Creates new instance of the <see cref="OpenClose"/> structure from date and two time points.
    /// </summary>
    /// <param name="date">Session date</param>
    /// <param name="open">Open time in EST time zone.</param>
    /// <param name="close">Close time in EST time zone.</param>
    internal OpenClose(
        DateOnly date,
        TimeOnly open,
        TimeOnly close)
    {
        OpenEst = CustomTimeZone.AsDateTimeOffset(date, open);
        CloseEst = CustomTimeZone.AsDateTimeOffset(date, close);
    }

    /// <summary>
    /// Gets open time in EST time zone.
    /// </summary>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public DateTimeOffset OpenEst { get; init; }

    /// <summary>
    /// Gets close time in EST time zone.
    /// </summary>
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public DateTimeOffset CloseEst { get; init; }

    /// <summary>
    /// Gets session open and close time as <see cref="Interval{DateTime}"/> instance with UTC times.
    /// </summary>
    /// <returns></returns>
    [UsedImplicitly]
    public Interval<DateTime> ToInterval() =>
        new(OpenEst.UtcDateTime, CloseEst.UtcDateTime);

    /// <summary>
    /// Gets session open and close time as <see cref="Interval{DateTime}"/> instance with UTC times.
    /// </summary>
    public static implicit operator Interval<DateTime>(
        OpenClose openClose) =>
        openClose.ToInterval();
}
