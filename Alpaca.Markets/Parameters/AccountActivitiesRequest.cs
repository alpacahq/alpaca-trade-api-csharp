namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListAccountActivitiesAsync(AccountActivitiesRequest,CancellationToken)"/> call.
/// </summary>
public sealed class AccountActivitiesRequest
{
    private readonly HashSet<AccountActivityType> _accountActivityTypes = [];

    /// <summary>
    /// Creates new instance of <see cref="AccountActivitiesRequest"/> object for all activity types.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public AccountActivitiesRequest()
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="AccountActivitiesRequest"/> object for a single activity types.
    /// </summary>
    /// <param name="activityType">The activity type you want to view entries for.</param>
    public AccountActivitiesRequest(
        AccountActivityType activityType) =>
        _accountActivityTypes.Add(activityType);

    /// <summary>
    /// Creates new instance of <see cref="AccountActivitiesRequest"/> object for several activity types.
    /// </summary>
    /// <param name="activityTypes">The list of activity types you want to view entries for.</param>
    public AccountActivitiesRequest(
        IEnumerable<AccountActivityType> activityTypes) =>
        _accountActivityTypes.UnionWith(activityTypes);

    /// <summary>
    /// Gets the activity types you want to view entries for. Empty list means 'all activity types'.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<AccountActivityType> ActivityTypes => _accountActivityTypes;

    /// <summary>
    /// Gets the date for which you want to see activities.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? Date { get; private set; }

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateTime> TimeInterval { get; private set; }

    /// <summary>
    /// Gets or sets the sorting direction for results.
    /// </summary>
    [UsedImplicitly]
    public SortDirection? Direction { get; [UsedImplicitly] set; }

    /// <summary>
    /// Gets or sets the maximum number of entries to return in the response.
    /// </summary>
    [UsedImplicitly]
    public Int64? PageSize { get; [UsedImplicitly] set; }

    /// <summary>
    /// Gets or sets the ID of the end of your current page of results.
    /// </summary>
    [UsedImplicitly]
    public String? PageToken { get; [UsedImplicitly] set; }

    /// <summary>
    /// Sets filtering for single <paramref name="date"/> activities.
    /// </summary>
    /// <param name="date">Target date for filtering activities.</param>
    /// <returns>Fluent interface method return same <see cref="AccountActivitiesRequest"/> instance.</returns>
    [UsedImplicitly]
    public AccountActivitiesRequest SetSingleDate(
        DateOnly date)
    {
        TimeInterval = new Interval<DateTime>();
        Date = date;
        return this;
    }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/account/activities",
            Query = await new QueryBuilder()
                .AddParameter("activity_types", ActivityTypes)
                .AddParameter("date", Date)
                .AddParameter("until", TimeInterval.Into, "O")
                .AddParameter("after", TimeInterval.From, "O")
                .AddParameter("direction", Direction)
                .AddParameter("pageSize", PageSize)
                .AddParameter("pageToken", PageToken)
                .AsStringAsync().ConfigureAwait(false)
        };

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AccountActivitiesRequest WithInterval(
        Interval<DateTime> value)
    {
        TimeInterval = value;
        Date = null;
        return this;
    }
}
