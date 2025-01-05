namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the stock merger information from Alpaca APIs.
/// </summary>
public interface IStockMerger
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
    /// Gets the stock merger process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the stock merger effective date
    /// </summary>
    public DateOnly EffectiveDate { get; }

    /// <summary>
    /// Gets the stock merger payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
