namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListCalendarAsync(CalendarRequest,CancellationToken)"/> call.
/// </summary>
public sealed class CalendarRequest :
#pragma warning disable CS0618 // Type or member is obsolete
    IRequestWithTimeInterval<IInclusiveTimeInterval>
#pragma warning restore CS0618 // Type or member is obsolete
 {
    /// <summary>
    /// Creates new instance of <see cref="CalendarRequest"/> object with the
    /// <see cref="TimeInterval"/> property configured for the single day.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [UsedImplicitly]
    [Obsolete("Use another method overload that takes the DateOnly argument.", false)]
    public static CalendarRequest GetForSingleDay(DateTime date) =>
        new CalendarRequest().WithInterval(
            new Interval<DateOnly>(DateOnly.FromDateTime(date), DateOnly.FromDateTime(date)));

    /// <summary>
    /// Creates new instance of <see cref="CalendarRequest"/> object with the
    /// <see cref="TimeInterval"/> property configured for the single day.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static CalendarRequest GetForSingleDay(DateOnly date) =>
        new CalendarRequest().WithInterval(
            new Interval<DateOnly>(date, date));

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("Use the DateInterval property instead of this one.", false)]
    public Interval<DateTime> TimeInterval => DateInterval.AsTimeInterval();

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateOnly> DateInterval { get; private set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/calendar",
            Query = await new QueryBuilder()
                .AddParameter("start", DateInterval.From)
                .AddParameter("end", DateInterval.Into)
                .AsStringAsync().ConfigureAwait(false)
        };

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CalendarRequest WithInterval(
        Interval<DateTime> value)
    {
        DateInterval = value.AsDateInterval();
        return this;
    }

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CalendarRequest WithInterval(
        Interval<DateOnly> value)
    {
        DateInterval = value;
        return this;
    }

    [Obsolete("Use WithInterval method instead of this one.", false)]
    void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
        IInclusiveTimeInterval value) =>
        WithInterval(new Interval<DateOnly>(value?.From.AsDateOnly(), value?.Into.AsDateOnly()));
}
