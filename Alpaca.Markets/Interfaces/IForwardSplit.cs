namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the forward split information from Alpaca APIs.
/// </summary>
public interface IForwardSplit
{
    /// <summary>
    /// Gets the symbol
    /// </summary>
    public String Symbol { get; }

    /// <summary>
    /// Gets the forward split new rate
    /// </summary>
    public Decimal NewRate { get; }

    /// <summary>
    /// Gets the forward split old rate
    /// </summary>
    public Decimal OldRate { get; }

    /// <summary>
    /// Gets the forward split process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the forward split execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the forward split record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the forward split payable date
    /// </summary>
    public DateOnly? PayableDate { get; }

    /// <summary>
    /// Gets the forward split due bill redemption date
    /// </summary>
    public DateOnly? DueBillRedemptionDate { get; }
}
