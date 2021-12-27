namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.GetPortfolioHistoryAsync(PortfolioHistoryRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class PortfolioHistoryRequest :
#pragma warning disable CS0618 // Type or member is obsolete
    IRequestWithTimeInterval<IInclusiveTimeInterval>
#pragma warning restore CS0618 // Type or member is obsolete
{
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

    /// <summary>
    /// Gets or sets the time frame value for desired history. Default value (if <c>null</c>) is 1 minute
    /// for a period shorter than 7 days, 15 minutes for a period less than 30 days, or 1 day for a longer period.
    /// </summary>
    [UsedImplicitly]
    public TimeFrame? TimeFrame { get; set; }

    /// <summary>
    /// Gets or sets period value for desired history. Default value (if <c>null</c>) is 1 month.
    /// </summary>
    [UsedImplicitly]
    public HistoryPeriod? Period { get; set; }

    /// <summary>
    /// Gets or sets flags, indicating that include extended hours included in the result.
    /// This is effective only for time frame less than 1 day.
    /// </summary>
    [UsedImplicitly]
    public Boolean? ExtendedHours { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/account/portfolio/history",
            Query = await new QueryBuilder()
                .AddParameter("start_date", DateInterval.From)
                .AddParameter("end_date", DateInterval.Into)
                .AddParameter("period", Period?.ToString())
                // ReSharper disable once StringLiteralTypo
                .AddParameter("timeframe", TimeFrame)
                .AddParameter("extended_hours", ExtendedHours)
                .AsStringAsync().ConfigureAwait(false)
        };

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PortfolioHistoryRequest WithInterval(
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
    public PortfolioHistoryRequest WithInterval(
        Interval<DateOnly> value)
    {
        DateInterval = value;
        return this;
    }

    [Obsolete("Use WithInterval method instead of this one.", false)]
    void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
        IInclusiveTimeInterval value) => WithInterval(value.AsDateOnlyInterval());
}
