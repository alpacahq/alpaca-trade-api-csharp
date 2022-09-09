namespace Alpaca.Markets;

/// <summary>
/// Encapsulates base logic for all historical crypto data requests on Alpaca Data API v2.
/// </summary>
public abstract class HistoricalCryptoRequestBase : HistoricalRequestBase
{
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
    /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", true)]
    protected internal HistoricalCryptoRequestBase(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : this(symbols, timeInterval.EnsureNotNull().AsDateTimeInterval())
    {
    }

    /// <summary>
    /// Gets crypto exchanges list for data retrieval (empty list means 'all exchanges').
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property is not supported by API anymore and will be removed in the next major release.", true)]
    public IReadOnlyCollection<CryptoExchange> Exchanges => Array.Empty<CryptoExchange>();

    internal override Boolean HasSingleSymbol => false;
}
