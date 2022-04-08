﻿namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalQuotesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalQuotesRequest, IQuote>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalQuotesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this(new[] { symbol }, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalQuotesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalQuotesRequest(
        String symbol,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <inheritdoc />
    protected override String LastPathSegment => "quotes";

    HistoricalQuotesRequest IHistoricalRequest<HistoricalQuotesRequest, IQuote>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalQuotesRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto())
            .WithPageSize(this.GetPageSize());
}
