namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalOptionQuotesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalOptionQuotesRequest, IQuote>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this([symbol.EnsureNotNull()], from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this([symbol.EnsureNotNull()], timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        String symbol)
        : this([symbol.EnsureNotNull()])
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols.EnsureNotNull(), from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols.EnsureNotNull(), timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalOptionQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalOptionQuotesRequest(
        IEnumerable<String> symbols)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>())
    {
    }

    /// <inheritdoc />
    protected override String LastPathSegment => "quotes";

    /// <inheritdoc />
    internal override Boolean HasSingleSymbol => false;

    HistoricalOptionQuotesRequest IHistoricalRequest<HistoricalOptionQuotesRequest, IQuote>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalOptionQuotesRequest(Symbols, TimeInterval)
            {
                SortDirection = SortDirection
            }
            .WithPageSize(this.GetPageSize());
}