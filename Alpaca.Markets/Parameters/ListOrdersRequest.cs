namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListOrdersAsync(ListOrdersRequest,CancellationToken)"/> call.
/// </summary>
public sealed class ListOrdersRequest : Validation.IRequest,
#pragma warning disable CS0618 // Type or member is obsolete
    IRequestWithTimeInterval<IExclusiveTimeInterval>
#pragma warning restore CS0618 // Type or member is obsolete
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets order status for filtering.
    /// </summary>
    [UsedImplicitly]
    public OrderStatusFilter? OrderStatusFilter { get; set; }

    /// <summary>
    /// Gets or sets the chronological order of response based on the submission time.
    /// </summary>
    [UsedImplicitly]
    public SortDirection? OrderListSorting { get; set; }

    /// <summary>
    /// Gets exclusive date time interval for filtering orders in response.
    /// </summary>
    [UsedImplicitly]
    public Interval<DateTime> TimeInterval { get; private set; }

    /// <summary>
    /// Gets or sets maximal number of orders in response.
    /// </summary>
    [UsedImplicitly]
    public Int64? LimitOrderNumber { get; set; }

    /// <summary>
    /// Gets or sets flag for rolling up multi-leg orders under the <see cref="IOrder.Legs"/> property of primary order.
    /// </summary>
    [UsedImplicitly]
    public Boolean? RollUpNestedOrders { get; set; }

    /// <summary>
    /// Gets list of symbols used for filtering the resulting list, if empty - orders for all symbols will be included.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Adds a single <paramref name="symbol"/> item into the <see cref="Symbols"/> list.
    /// </summary>
    /// <param name="symbol">Single symbol name for filtering.</param>
    /// <returns>Fluent interface, returns the original <see cref="ListOrdersRequest"/> instance.</returns>
    [UsedImplicitly]
    public ListOrdersRequest WithSymbol(String symbol)
    {
        _symbols.Add(symbol.EnsureNotNull(nameof(symbol)));
        return this;
    }

    /// <summary>
    /// Adds all items from the <paramref name="symbols"/> list into the <see cref="Symbols"/> list.
    /// </summary>
    /// <param name="symbols">List of symbol names for filtering.</param>
    /// <returns>Fluent interface, returns the original <see cref="ListOrdersRequest"/> instance.</returns>
    [UsedImplicitly]
    public ListOrdersRequest WithSymbols(IEnumerable<String> symbols)
    {
        _symbols.UnionWith(symbols.EnsureNotNull(nameof(symbols)));
        return this;
    }

    /// <summary>
    /// Sets time interval for filtering data returned by this request.
    /// /// </summary>
    /// <param name="value">New filtering interval.</param>
    /// <returns>Request with applied filtering.</returns>
    [UsedImplicitly]
    public ListOrdersRequest WithInterval(
        Interval<DateTime> value)
    {
        TimeInterval = value;
        return this;
    }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/orders",
            Query = await new QueryBuilder()
                .AddParameter("status", OrderStatusFilter)
                .AddParameter("direction", OrderListSorting)
                .AddParameter("until", TimeInterval.Into, "O")
                .AddParameter("after", TimeInterval.From, "O")
                .AddParameter("limit", LimitOrderNumber)
                .AddParameter("nested", RollUpNestedOrders)
                .AddParameter("symbols", _symbols)
                .AsStringAsync().ConfigureAwait(false)
        };

    IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
    {
        if (_symbols.Contains(String.Empty))
        {
            yield return new RequestValidationException("Symbol name shouldn't be empty.", nameof(Symbols));
        }
    }

    [ExcludeFromCodeCoverage]
    [Obsolete("Use WithInterval method instead of this one.", false)]
    void IRequestWithTimeInterval<IExclusiveTimeInterval>.SetInterval(
        IExclusiveTimeInterval value) => WithInterval(value.AsDateTimeInterval());
}
