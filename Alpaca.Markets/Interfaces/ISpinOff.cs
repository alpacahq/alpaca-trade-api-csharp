namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the spin-off information from Alpaca APIs.
/// </summary>
public interface ISpinOff
{
    /// <summary>
    /// Gets the source symbol
    /// </summary>
    public String SourceSymbol { get; }

    /// <summary>
    /// Gets the new symbol
    /// </summary>
    public String NewSymbol { get; }

    /// <summary>
    /// Gets the spin-off source rate
    /// </summary>
    public Decimal SourceRate { get; }

    /// <summary>
    /// Gets the spin-off new rate
    /// </summary>
    public Decimal NewRate { get; }

    /// <summary>
    /// Gets the spin-off process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the spin-off execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the spin-off payable date
    /// </summary>
    public DateOnly? PayableDate { get; }

    /// <summary>
    /// Gets the spin-off record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the spin-off due bill redemption date
    /// </summary>
    public DateOnly? DueBillRedemptionDate { get; }
}
