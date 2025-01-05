namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the redemption information from Alpaca APIs.
/// </summary>
public interface IRedemption
{
    /// <summary>
    /// Gets the symbol
    /// </summary>
    public String Symbol { get; }

    /// <summary>
    /// Gets the dividend rate
    /// </summary>
    public Decimal Rate { get; }

    /// <summary>
    /// Gets the redemption process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the redemption payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}
