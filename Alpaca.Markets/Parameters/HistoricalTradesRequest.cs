namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalTradesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalTradesRequest, ITrade>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalTradesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this(new[] { symbol }, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalTradesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalTradesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalTradesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <inheritdoc />
    protected override String LastPathSegment => "trades";

    HistoricalTradesRequest IHistoricalRequest<HistoricalTradesRequest, ITrade>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalTradesRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto())
            .WithPageSize(this.GetPageSize());
}
