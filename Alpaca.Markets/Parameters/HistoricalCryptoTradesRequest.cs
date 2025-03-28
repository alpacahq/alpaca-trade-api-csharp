﻿namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalCryptoTradesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalCryptoTradesRequest, ITrade>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this([symbol.EnsureNotNull()], from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this([symbol.EnsureNotNull()], timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        String symbol)
        : this([symbol.EnsureNotNull()])
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols.EnsureNotNull(), from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols.EnsureNotNull(), timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>())
    {
    }

    internal override Boolean HasSingleSymbol => false;

    /// <inheritdoc />
    protected override String LastPathSegment => "trades";

    HistoricalCryptoTradesRequest IHistoricalRequest<HistoricalCryptoTradesRequest, ITrade>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalCryptoTradesRequest(Symbols, TimeInterval)
            {
                SortDirection = SortDirection
            }
            .WithPageSize(this.GetPageSize());
}
