﻿namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalAuctionsRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalAuctionsRequest, IAuction>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this([symbol.EnsureNotNull()], from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this([symbol.EnsureNotNull()], timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        String symbol)
        : this([symbol.EnsureNotNull()])
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols.EnsureNotNull(), from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols.EnsureNotNull(), timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalAuctionsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalAuctionsRequest(
        IEnumerable<String> symbols)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>())
    {
    }

    /// <summary>
    /// Gets or sets the optional parameter for mapping symbol to contract by a specific date.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? UseSymbolAsOfTheDate { get; set; }

    /// <inheritdoc />
    protected override String LastPathSegment => "auctions";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) => 
        queryBuilder
            .AddParameter("asof", UseSymbolAsOfTheDate);

    HistoricalAuctionsRequest IHistoricalRequest<HistoricalAuctionsRequest, IAuction>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalAuctionsRequest(Symbols, TimeInterval)
                { UseSymbolAsOfTheDate = UseSymbolAsOfTheDate }
            .WithPageSize(this.GetPageSize());
}
