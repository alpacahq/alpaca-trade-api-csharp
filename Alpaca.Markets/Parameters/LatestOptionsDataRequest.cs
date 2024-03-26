namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest options data requests on Alpaca Data API v2.
/// </summary>
public sealed class LatestOptionsDataRequest : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="LatestOptionsDataRequest"/> object.
    /// </summary>
    /// <param name="symbols">Options symbols list for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public LatestOptionsDataRequest(
        IEnumerable<String> symbols) =>
        _symbols.UnionWith(symbols.EnsureNotNull());

    /// <summary>
    /// Gets options symbols list for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets options feed for data retrieval.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    public OptionsFeed? OptionsFeed { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient,
        String lastPathSegment) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("symbols", Symbols.ToList())
                .AddParameter("feed", OptionsFeed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(lastPathSegment);

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolsList();
        yield return Symbols.TryValidateSymbolName();
    }
}
