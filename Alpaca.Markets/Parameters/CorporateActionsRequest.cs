namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaDataClient.ListCorporateActionsAsync(CorporateActionsRequest,CancellationToken)"/> call.
/// </summary>
public sealed class CorporateActionsRequest : Validation.IRequest, IHistoricalRequest<CorporateActionsRequest, ICorporateActionsResponse>
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    private readonly HashSet<CorporateActionFilterType> _types = [];

    /// <summary>
    /// Gets asset symbols list for data retrieval (all symbols if list is empty).
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets corporate action types list for data retrieval (all types if list is empty).
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<CorporateActionFilterType> Types => _types;

    /// <summary>
    /// Gets the date range when searching corporate actions history (current day if empty).
    /// </summary>
    [UsedImplicitly]
    public Interval<DateOnly> DateInterval { get; private set; }

    /// <summary>
    /// Gets the pagination parameters for the request (page size and token).
    /// </summary>
    public Pagination Pagination { get; } = new();

    /// <summary>
    /// Gets or sets the result sorting direction (sort fields is timestamp).
    /// </summary>
    [UsedImplicitly]
    public SortDirection? SortDirection { get; set; }

    /// <summary>
    /// Adds several symbols into the <see cref="Symbols"/> filter list.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <returns>Fluent interface - returns the modified request object.</returns>
    public CorporateActionsRequest WithSymbols(
        IEnumerable<String> symbols)
    {
        _symbols.UnionWith(symbols.EnsureNotNull());
        return this;
    }

    /// <summary>
    /// Adds a single symbol into the <see cref="Symbols"/> filter list.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <returns>Fluent interface - returns the modified request object.</returns>
    public CorporateActionsRequest WithSymbol(
        String symbol)
    {
        _symbols.Add(symbol.EnsureNotNull());
        return this;
    }

    /// <summary>
    /// Adds several items into the <see cref="Types"/> filter list.
    /// </summary>
    /// <param name="types">Corporate action types for data retrieval.</param>
    /// <returns>Fluent interface - returns the modified request object.</returns>
    public CorporateActionsRequest WithTypes(
        IEnumerable<CorporateActionFilterType> types)
    {
        _types.UnionWith(types.EnsureNotNull());
        return this;
    }

    /// <summary>
    /// Adds a single item into the <see cref="Types"/> filter list.
    /// </summary>
    /// <param name="type">Corporate action type for data retrieval.</param>
    /// <returns>Fluent interface - returns the modified request object.</returns>
    public CorporateActionsRequest WithType(
        CorporateActionFilterType type)
    {
        _types.Add(type);
        return this;
    }

    /// <summary>
    /// Sets the <see cref="DateInterval"/> filter value for the request.
    /// </summary>
    /// <param name="dateInterval">The inclusive date interval for data retrieval.</param>
    /// <returns>Fluent interface - returns the modified request object.</returns>
    public CorporateActionsRequest WithDateInterval(
        Interval<DateOnly> dateInterval)
    {
        DateInterval = dateInterval;
        return this;
    }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "../v1/corporate-actions",
            Query = await Pagination.QueryBuilder
                .AddParameter("symbols", Symbols)
                .AddParameter("types", Types)
                .AddParameter("start", DateInterval.From)
                .AddParameter("end", DateInterval.Into)
                .AddParameter("sort", SortDirection)
                .AsStringAsync().ConfigureAwait(false)
        };

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Pagination.TryValidatePageSize(Pagination.MaxPageSize);
        yield return Symbols.TryValidateSymbolName();
    }

    CorporateActionsRequest IHistoricalRequest<CorporateActionsRequest, ICorporateActionsResponse>.GetValidatedRequestWithoutPageToken() =>
        new CorporateActionsRequest
            {
                SortDirection = SortDirection,
                DateInterval = DateInterval
            }
            .WithTypes(Types).WithSymbols(Symbols)
            .WithPageSize(Pagination.MaxCorporateActionsPageSize);
}
