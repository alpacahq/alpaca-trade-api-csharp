namespace Alpaca.Markets;

/// <summary>
/// Options Greeks are a set of risk measures that are used in the options market to evaluate the risk and reward of an option.
/// </summary>
[CLSCompliant(false)]
public interface IGreeks
{
    /// <summary>
    /// Gets the rate of change of an option's price relative to a change in the price of the underlying asset.
    /// </summary>
    [UsedImplicitly]
    Decimal? Delta { get; }

    /// <summary>
    /// Gets the rate of change in an option's delta relative to a change in the price of the underlying asset.
    /// </summary>
    [UsedImplicitly]
    Decimal? Gamma { get; }

    /// <summary>
    /// Gets the rate of change in an option's price relative to a change in the risk-free rate of interest.
    /// </summary>
    [UsedImplicitly]
    Decimal? Rho { get; }

    /// <summary>
    /// Gets the rate of change in an option's price relative to a change in time.
    /// </summary>
    [UsedImplicitly]
    Decimal? Theta { get; }

    /// <summary>
    /// Gets the rate of change in an option's price relative to a change in the volatility of the underlying asset.
    /// </summary>
    [UsedImplicitly]
    Decimal? Vega { get; }
}