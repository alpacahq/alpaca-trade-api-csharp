namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the single market mover information from Alpaca APIs.
/// </summary>
public interface IMarketMover
{
    /// <summary>
    /// Gets the instrument symbol name.
    /// </summary>
    String Symbol { get; }

    /// <summary>
    /// Gets the current instrument price value.
    /// </summary>
    Decimal Price { get; }

    /// <summary>
    /// Gets the current instrument price change value.
    /// </summary>
    Decimal Change { get; }

    /// <summary>
    /// Gets the current instrument price change value in percents.
    /// </summary>
    Decimal PercentChange { get; }
}
