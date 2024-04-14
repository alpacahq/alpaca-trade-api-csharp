namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.GetPortfolioHistoryAsync(PortfolioHistoryRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class PortfolioHistoryRequest : Validation.IRequest
{
    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use the Interval property instead of this one.", true)]
    public Interval<DateOnly> DateInterval => Interval.AsDateInterval();

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateTime> Interval { get; private set; }

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
    /// Gets or sets intraday reporting style. Make sense only if <see cref="TimeFrame"/> are equal to <see cref="TimeFrame.Day"/>.
    /// </summary>
    [UsedImplicitly]
    public IntradayReporting? IntradayReporting { get; set; }

    /// <summary>
    /// Gets or sets intraday profit/loss reset. Make sense only if <see cref="TimeFrame"/> are equal to <see cref="TimeFrame.Day"/>.
    /// </summary>
    [UsedImplicitly]
    public IntradayProfitLoss? IntradayProfitLoss { get; set; }

    /// <summary>
    /// Gets or sets flags, indicating that include extended hours included in the result.
    /// This is effective only for time frame less than 1 day.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use the DateInterval property instead of this one.", false)]
    public Boolean? ExtendedHours { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/account/portfolio/history",
            Query = await new QueryBuilder()
                .AddParameter("intraday_reporting", IntradayReporting)
                .AddParameter("period", Period?.ToString())
                .AddParameter("start", Interval.From, "O")
                .AddParameter("end", Interval.Into, "O")
                .AddParameter("pnl_reset", IntradayProfitLoss)
                // ReSharper disable once StringLiteralTypo
                .AddParameter("timeframe", TimeFrame)
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
        Interval = value;
        return this;
    }

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Obsolete("Use the override that gets Interval<DateTime> instead of this one.", true)]
    public PortfolioHistoryRequest WithInterval(
        Interval<DateOnly> value)
    {
        Interval = value.AsTimeInterval();
        return this;
    }

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Interval.TryValidateInterval();
        if (Period.HasValue && !Interval.IsEmpty())
        {
            yield return new RequestValidationException(
                "Both `Period` and `Interval` are set.", nameof(Period));
        }
    }
}
