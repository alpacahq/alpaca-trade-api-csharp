namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.GetPortfolioHistoryAsync(PortfolioHistoryRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class PortfolioHistoryRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>, IRequestWithDateInterval
{
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
        IInclusiveTimeInterval value) =>
        DateInterval = value.EnsureNotNull(nameof(value)).AsDateInterval();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void IRequestWithDateInterval.SetInterval(
        IDateInterval value) => DateInterval = value.EnsureNotNull(nameof(value));
}
