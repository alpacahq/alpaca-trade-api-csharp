namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListCalendarAsync(CalendarRequest,CancellationToken)"/> call.
/// </summary>
public sealed class CalendarRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>, IRequestWithDateInterval
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
        new CalendarRequest().SetDateInterval(
            DateOnly.FromDateTime(date), DateOnly.FromDateTime(date));

    /// <summary>
    /// Creates new instance of <see cref="CalendarRequest"/> object with the
    /// <see cref="TimeInterval"/> property configured for the single day.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [UsedImplicitly]
    public static CalendarRequest GetForSingleDay(DateOnly date) =>
        new CalendarRequest().SetDateInterval(date, date);

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("Use the DateInterval property instead of this one.", false)]
    public IInclusiveTimeInterval TimeInterval => DateInterval.AsTimeInterval();

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public IDateInterval DateInterval { get; private set; } = Markets.DateInterval.Empty;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
        IInclusiveTimeInterval value) =>
        DateInterval = value.EnsureNotNull(nameof(value)).AsDateInterval();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void IRequestWithDateInterval.SetInterval(
        IDateInterval value) => DateInterval = value.EnsureNotNull(nameof(value));
}
