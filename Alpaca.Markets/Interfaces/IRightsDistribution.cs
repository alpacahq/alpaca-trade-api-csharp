namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the right distribution information from Alpaca APIs.
/// </summary>
public interface IRightsDistribution
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
    /// Gets the right distribution rate
    /// </summary>
    public Decimal Rate { get; }

    /// <summary>
    /// Gets the right distribution process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the right distribution execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the right distribution payable date
    /// </summary>
    public DateOnly? PayableDate { get; }

    /// <summary>
    /// Gets the right distribution record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the right distribution expiration date
    /// </summary>
    public DateOnly? ExpirationDate { get; }
}
