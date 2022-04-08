﻿namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base logic for all historical crypto data requests on Alpaca Data API v2.
/// </summary>
public abstract class HistoricalCryptoRequestBase : HistoricalRequestBase
{
    private readonly HashSet<CryptoExchange> _exchanges = new();

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <param name="exchanges">Crypto exchanges list for data retrieval.</param>
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval,
        IEnumerable<CryptoExchange> exchanges)
        : base(symbols, timeInterval) =>
        _exchanges.UnionWith(exchanges);

    /// <summary>
    /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : this(symbols, timeInterval.AsDateTimeInterval())
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <param name="exchanges">Crypto exchanges list for data retrieval.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval,
        IEnumerable<CryptoExchange> exchanges)
        : this(symbols, timeInterval.AsDateTimeInterval(), exchanges)
    {
    }

    internal void CopyPagination(Pagination pagination)
    {
        Pagination.Token = pagination.Token;
        Pagination.Size = pagination.Size;
    }

    /// <summary>
    /// Gets crypto exchanges list for data retrieval (empty list means 'all exchanges').
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<CryptoExchange> Exchanges => _exchanges;

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) =>
        base.AddParameters(queryBuilder)
            .AddParameter("exchanges", Exchanges);
}
