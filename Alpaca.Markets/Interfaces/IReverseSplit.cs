namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the reverse split information from Alpaca APIs.
/// </summary>
public interface IReverseSplit
{
    /// <summary>
    /// Gets the symbol
    /// </summary>
    public String Symbol { get; }

    /// <summary>
    /// Gets the reverse split new rate
    /// </summary>
    public Decimal NewRate { get; }

    /// <summary>
    /// Gets the reverse split old rate
    /// </summary>
    public Decimal OldRate { get; }

    /// <summary>
    /// Gets the reverse split process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the reverse split execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the reverse split record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the reverse split payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
