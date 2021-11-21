namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListCalendarAsync(CalendarRequest,CancellationToken)"/> call.
/// </summary>
public sealed class CalendarRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
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
        new CalendarRequest().SetInclusiveTimeInterval(date.Date, date.Date);

    /// <summary>
    /// Creates new instance of <see cref="CalendarRequest"/> object with the
    /// <see cref="TimeInterval"/> property configured for the single day.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static CalendarRequest GetForSingleDay(DateOnly date) =>
        new CalendarRequest().SetInclusiveTimeInterval(
            date.ToDateTime(TimeOnly.MinValue), date.ToDateTime(TimeOnly.MinValue));

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    // TODO: olegra - good candidate for the DateOnly type usage
    public IInclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/calendar",
            Query = await new QueryBuilder()
                .AddParameter("start", asDateOnly(TimeInterval.From))
                .AddParameter("end", asDateOnly(TimeInterval.Into))
                .AsStringAsync().ConfigureAwait(false)
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
        IInclusiveTimeInterval value) => TimeInterval = value.EnsureNotNull(nameof(value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DateOnly? asDateOnly(DateTime? date) =>
        date.HasValue ? DateOnly.FromDateTime(date.Value) : null;
}
