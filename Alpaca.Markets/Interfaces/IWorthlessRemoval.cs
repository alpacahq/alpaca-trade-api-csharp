namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the worthless removal information from Alpaca APIs.
/// </summary>
public interface IWorthlessRemoval
{
    /// <summary>
    /// Gets the symbol
    /// </summary>
    public String Symbol { get; }

    /// <summary>
    /// Gets the worthless removal process date
    /// </summary>
    public DateOnly ProcessDate { get; }
}
