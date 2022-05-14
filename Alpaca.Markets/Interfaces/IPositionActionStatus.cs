namespace Alpaca.Markets;

/// <summary>
/// Encapsulates position action status information from Alpaca REST API.
/// </summary>
public interface IPositionActionStatus
{
    /// <summary>
    /// Gets processed position asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Returns <c>true</c> if requested action completed successfully.
    /// </summary>
    [UsedImplicitly]
    Boolean IsSuccess { get; }
}
