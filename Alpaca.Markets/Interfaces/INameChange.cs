namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the name change information from Alpaca APIs.
/// </summary>
public interface INameChange
{
    /// <summary>
    /// Gets the old symbol
    /// </summary>
    public String OldSymbol { get; }

    /// <summary>
    /// Gets the new symbol
    /// </summary>
    public String NewSymbol { get; }

    /// <summary>
    /// Gets the name change process date
    /// </summary>
    public DateOnly ProcessDate { get; }
}
