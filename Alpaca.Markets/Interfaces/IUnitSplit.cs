namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the unit split information from Alpaca APIs.
/// </summary>
public interface IUnitSplit
{
    /// <summary>
    /// Gets the new symbol
    /// </summary>
    public String NewSymbol { get; }

    /// <summary>
    /// Gets the old symbol
    /// </summary>
    public String OldSymbol { get; }

    /// <summary>
    /// Gets the alternate symbol
    /// </summary>
    public String AlternateSymbol { get; }

    /// <summary>
    /// Gets the unit split new rate
    /// </summary>
    public Decimal NewRate { get; }

    /// <summary>
    /// Gets the unit split old rate
    /// </summary>
    public Decimal OldRate { get; }

    /// <summary>
    /// Gets the unit split alternate rate
    /// </summary>
    public Decimal AlternateRate { get; }

    /// <summary>
    /// Gets the unit split process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the unit split effective date
    /// </summary>
    public DateOnly EffectiveDate { get; }

    /// <summary>
    /// Gets the unit split payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
