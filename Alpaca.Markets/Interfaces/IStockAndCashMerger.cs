namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the stock and cash merger information from Alpaca APIs.
/// </summary>
public interface IStockAndCashMerger
{
    /// <summary>
    /// Gets the acquirer symbol
    /// </summary>
    public String AcquirerSymbol { get; }

    /// <summary>
    /// Gets the acquiree symbol
    /// </summary>
    public String AcquireeSymbol { get; }

    /// <summary>
    /// Gets the acquirer rate
    /// </summary>
    public Decimal AcquirerRate { get; }

    /// <summary>
    /// Gets the acquiree rate
    /// </summary>
    public Decimal AcquireeRate { get; }

    /// <summary>
    /// Gets the cash rate
    /// </summary>
    public Decimal CashRate { get; }

    /// <summary>
    /// Gets the stock and cash merger process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the stock and cash merger effective date
    /// </summary>
    public DateOnly EffectiveDate { get; }

    /// <summary>
    /// Gets the stock and cash merger payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
