namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the cash merger information from Alpaca APIs.
/// </summary>
public interface ICashMerger
{
    /// <summary>
    /// Gets the acquirer symbol
    /// </summary>
    public String? AcquirerSymbol { get; }

    /// <summary>
    /// Gets the acquiree symbol
    /// </summary>
    public String AcquireeSymbol { get; }

    /// <summary>
    /// Gets the cash merger rate
    /// </summary>
    public Decimal Rate { get; }

    /// <summary>
    /// Gets the cash merger process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the cash merger effective date
    /// </summary>
    public DateOnly EffectiveDate { get; }

    /// <summary>
    /// Gets the cash merger payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
