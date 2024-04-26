namespace Alpaca.Markets;

/// <summary>
/// Encapsulates option snapshot information from the Alpaca REST API.
/// </summary>
[CLSCompliant(false)]
public interface IOptionSnapshot
{
    /// <summary>
    /// Gets the snapshot's option symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets the latest quote information.
    /// </summary>
    [UsedImplicitly]
    IQuote? Quote { get; }

    /// <summary>
    /// Gets the latest trade information.
    /// </summary>
    [UsedImplicitly]
    ITrade? Trade { get; }

    /// <summary>
    /// Gets the option greeks data.
    /// </summary>
    [UsedImplicitly]
    IGreeks? Greeks { get; }

    /// <summary>
    /// Gets the implied volatility of the option.
    /// </summary>
    [UsedImplicitly]
    Decimal? ImpliedVolatility { get; }
}
