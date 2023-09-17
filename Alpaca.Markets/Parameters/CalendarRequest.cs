namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListIntervalCalendarAsync(CalendarRequest,CancellationToken)"/> call.
/// </summary>
public sealed class CalendarRequest
 {
     /// <summary>
     /// Creates new instance of <see cref="CalendarRequest"/> object.
     /// </summary>
     public CalendarRequest()
     {
     }

     /// <summary>
     /// Creates new instance of <see cref="CalendarRequest"/> object.
     /// </summary>
     /// <param name="from">Start date for the resulting data set.</param>
     /// <param name="into">End date for the resulting data set.</param>
     public CalendarRequest(
         DateOnly? from,
         DateOnly? into)
         : this(new Interval<DateOnly>(from, into))
     {
     }

     /// <summary>
     /// Creates new instance of <see cref="CalendarRequest"/> object.
     /// </summary>
     /// <param name="dateInterval">Initial value of the <see cref="DateInterval"/> property.</param>
     public CalendarRequest(
         Interval<DateOnly> dateInterval) =>
         DateInterval = dateInterval;

    /// <summary>
    /// Creates new instance of <see cref="CalendarRequest"/> object with the
    /// <see cref="DateInterval"/> property configured for the single day.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static CalendarRequest GetForSingleDay(DateOnly date) =>
        new(new Interval<DateOnly>(date));

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
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CalendarRequest WithInterval(
        Interval<DateTime> value) =>
        WithInterval(value.AsDateInterval());

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
}
