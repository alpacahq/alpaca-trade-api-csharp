namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListAssetsAsync(AssetsRequest,CancellationToken)"/> call.
/// </summary>
public sealed class AnnouncementsRequest : Validation.IRequest
{
    private readonly HashSet<CorporateActionType> _corporateActionTypes = new ();

    /// <summary>
    /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
    /// </summary>
    /// <param name="corporateActionType">Single corporate action type for filtering.</param>
    /// <param name="dateInterval">Date range when searching corporate action announcements.</param>
    public AnnouncementsRequest(
        CorporateActionType corporateActionType,
        Interval<DateOnly> dateInterval)
    {
        _corporateActionTypes.Add(corporateActionType);
        DateInterval = dateInterval;
    }

    /// <summary>
    /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
    /// </summary>
    /// <param name="corporateActionTypes">List of the corporate action types for filtering.</param>
    /// <param name="dateInterval">Date range when searching corporate action announcements.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="corporateActionTypes"/> argument is <c>null</c>.
    /// </exception>
    public AnnouncementsRequest(
        IEnumerable<CorporateActionType> corporateActionTypes,
        Interval<DateOnly> dateInterval)
    {
        _corporateActionTypes.UnionWith(corporateActionTypes.EnsureNotNull());
        DateInterval = dateInterval;
    }

    /// <summary>
    /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
    /// </summary>
    /// <param name="corporateActionType">Single corporate action type for filtering.</param>
    /// <param name="timeInterval">Date range when searching corporate action announcements.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("This constructor will be removed in the next major release. Use overload that takes Interval<DateOnly> argument.", true)]
    public AnnouncementsRequest(
        CorporateActionType corporateActionType,
        IInclusiveTimeInterval timeInterval)
        : this (corporateActionType, timeInterval.EnsureNotNull().AsDateOnlyInterval())
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
    /// </summary>
    /// <param name="corporateActionTypes">List of the corporate action types for filtering.</param>
    /// <param name="timeInterval">Date range when searching corporate action announcements.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("This constructor will be removed in the next major release. Use overload that takes Interval<DateOnly> argument.", true)]
    public AnnouncementsRequest(
        IEnumerable<CorporateActionType> corporateActionTypes,
        IInclusiveTimeInterval timeInterval)
        : this (corporateActionTypes, timeInterval.EnsureNotNull().AsDateOnlyInterval())
    {
    }

    /// <summary>
    /// Gets the list of the corporate action types for filtering.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<CorporateActionType> CorporateActionTypes => _corporateActionTypes;

    /// <summary>
    /// Gets the date range when searching corporate action announcements.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    [Obsolete("This property will be removed in the next major release. Use the DateInterval property instead.", true)]
    public IInclusiveTimeInterval TimeInterval =>
        throw new InvalidOperationException("Use the DateInterval property instead.");

    /// <summary>
    /// Gets the date range when searching corporate action announcements.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateOnly> DateInterval { get; }

    /// <summary>
    /// Gets or sets the type of date for filtering by <see cref="DateInterval"/> parameter.
    /// </summary>
    [UsedImplicitly]
    public CorporateActionDateType? DateType { get; set; }

    /// <summary>
    /// Gets or sets the symbol of the company initiating the announcement.
    /// </summary>
    [UsedImplicitly]
    public String? Symbol { get; set; }

    /// <summary>
    /// Gets or sets the CUSIP of the company initiating the announcement.
    /// </summary>
    [UsedImplicitly]
    public String? Cusip { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new (httpClient.BaseAddress!)
        {
            Path = "v2/corporate_actions/announcements",
            Query = await new QueryBuilder()
                .AddParameter("ca_types", CorporateActionTypes)
                .AddParameter("since", DateInterval.From)
                .AddParameter("until", DateInterval.Into)
                .AddParameter("date_type", DateType)
                .AddParameter("symbol", Symbol)
                .AddParameter("cusip", Cusip)
                .AsStringAsync().ConfigureAwait(false)
        };

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return CorporateActionTypes.TryValidateCollection();
        yield return DateInterval.TryValidateInterval();
        yield return Symbol?.TryValidateSymbolName();
        yield return Cusip?.TryValidateSymbolName();
    }
}
