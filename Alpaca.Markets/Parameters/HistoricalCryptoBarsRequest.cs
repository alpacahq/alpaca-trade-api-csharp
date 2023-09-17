namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
public sealed class HistoricalCryptoBarsRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalCryptoBarsRequest, IBar>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : this(new[] { symbol.EnsureNotNull() }, from, into, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame)
        : this(new[] { symbol.EnsureNotNull() }, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), from, into) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), timeInterval) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>()) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Gets type of time bars for retrieval.
    /// </summary>
    [UsedImplicitly]
    public BarTimeFrame TimeFrame { get; }

    internal override Boolean HasSingleSymbol => false;

    /// <inheritdoc />
    protected override String LastPathSegment => "bars";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) =>
        base.AddParameters(queryBuilder)
            // ReSharper disable once StringLiteralTypo
            .AddParameter("timeframe", TimeFrame.ToString());

    HistoricalCryptoBarsRequest IHistoricalRequest<HistoricalCryptoBarsRequest, IBar>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalCryptoBarsRequest(Symbols, TimeInterval, TimeFrame)
            {
                SortDirection = SortDirection
            }
            .WithPageSize(this.GetPageSize());
}
