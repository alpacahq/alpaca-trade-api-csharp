namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="AlpacaDataClient.ListNewsArticlesAsync(NewsArticlesRequest,System.Threading.CancellationToken)"/> call.
/// </summary>
public sealed class NewsArticlesRequest : Validation.IRequest, IHistoricalRequest<NewsArticlesRequest, INewsArticle>
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="NewsArticlesRequest"/> object.
    /// </summary>
    public NewsArticlesRequest()
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="NewsArticlesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public NewsArticlesRequest(
        IEnumerable<String> symbols) =>
        _symbols.UnionWith(symbols.EnsureNotNull());

    /// <summary>
    /// Gets assets names list for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets or sets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateTime>? TimeInterval { get; set; }

    /// <summary>
    /// Gets or sets articles sorting (by <see cref="INewsArticle.UpdatedAtUtc"/> property) direction.
    /// </summary>
    [UsedImplicitly]
    public SortDirection? SortDirection { get; set; }

    /// <summary>
    /// Gets or sets flag for sending <see cref="INewsArticle.Content"/> property value for each news article.
    /// </summary>
    [UsedImplicitly]
    public Boolean? SendFullContentForItems { get; set; }

    /// <summary>
    /// Gets or sets flag for excluding news articles that do not contain <see cref="INewsArticle.Content"/>
    /// property value (just <see cref="INewsArticle.Headline"/> and <see cref="INewsArticle.Summary"/> values).
    /// </summary>
    [UsedImplicitly]
    public Boolean? ExcludeItemsWithoutContent { get; set; }

    /// <summary>
    /// Gets the pagination parameters for the request (page size and token).
    /// </summary>
    [UsedImplicitly]
    public Pagination Pagination { get; } = new();

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await Pagination.QueryBuilder
                .AddParameter("symbols", Symbols)
                .AddParameter("start", TimeInterval?.From, "O")
                .AddParameter("end", TimeInterval?.Into, "O")
                .AddParameter("sort", SortDirection)
                .AddParameter("include_content", SendFullContentForItems)
                .AddParameter("exclude_contentless", ExcludeItemsWithoutContent)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath("../../v1beta1/news");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Pagination.TryValidatePageSize(Pagination.MaxNewsPageSize);
        yield return Symbols.TryValidateSymbolName();
    }

    NewsArticlesRequest IHistoricalRequest<NewsArticlesRequest, INewsArticle>.GetValidatedRequestWithoutPageToken() =>
        new NewsArticlesRequest(Symbols)
            {
                TimeInterval = TimeInterval,
                SortDirection = SortDirection,
                SendFullContentForItems = SendFullContentForItems,
                ExcludeItemsWithoutContent = ExcludeItemsWithoutContent
            }
            .WithPageSize(Pagination.Size ?? Pagination.MaxNewsPageSize);
}
