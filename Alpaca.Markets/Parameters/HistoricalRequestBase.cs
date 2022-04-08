﻿namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base logic for all historical data requests on Alpaca Data API v2.
/// </summary>
public abstract class HistoricalRequestBase : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    protected internal HistoricalRequestBase(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : this(symbols, new Interval<DateTime>(from, into))
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    protected internal HistoricalRequestBase(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
    {
        _symbols.UnionWith(symbols.EnsureNotNull());
        TimeInterval = timeInterval;
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    protected internal HistoricalRequestBase(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : this (symbols, timeInterval.AsDateTimeInterval())
    {
    }

    /// <summary>
    /// Gets asset symbol for data retrieval.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    [Obsolete("This property is obsolete and will be removed in the next major version of SDK. Use the `Symbols` property instead of this one.", false)]
    public String Symbol => _symbols.FirstOrDefault() ?? String.Empty;

    /// <summary>
    /// Gets asset symbols list for data retrieval.
    /// </summary>
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets inclusive date interval for filtering items in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateTime> TimeInterval { get; }

    /// <summary>
    /// Gets the pagination parameters for the request (page size and token).
    /// </summary>
    public Pagination Pagination { get; } = new();

    /// <summary>
    /// Gets the last part of the full REST endpoint URL path.
    /// </summary>
    protected abstract String LastPathSegment { get; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await AddParameters(Pagination.QueryBuilder
                    .AddParameter("symbols",
                        HasSingleSymbol ? Array.Empty<String>() : Symbols)
                    .AddParameter("start", TimeInterval.From, "O")
                    .AddParameter("end", TimeInterval.Into, "O"))
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(HasSingleSymbol
            ? $"{Symbols.First()}/{LastPathSegment}"
            : $"{LastPathSegment}");

    internal virtual QueryBuilder AddParameters(
        QueryBuilder queryBuilder) => queryBuilder;

    internal Boolean HasSingleSymbol => Symbols.Count == 1;

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Pagination.TryValidatePageSize(Pagination.MaxPageSize);
        yield return Symbols.TryValidateSymbolsList();
        yield return Symbols.TryValidateSymbolName();
    }
}
