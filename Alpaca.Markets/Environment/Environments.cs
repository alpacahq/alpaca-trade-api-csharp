namespace Alpaca.Markets;

/// <summary>
/// Provides single entry point for obtaining information about different environments.
/// </summary>
public static class Environments
{
    /// <summary>
    /// Gets live trading environment. 
    /// </summary>
    public static IEnvironment Live { get; } = new LiveEnvironment();

    /// <summary>
    /// Gets paper trading environment. Paper trading is a simulation environment that does not use real money.
    /// See <a href="https://alpaca.markets/docs/trading/paper-trading/">Alpaca Paper Trading Documentation</a> for more information.
    /// </summary>
    [UsedImplicitly]
    public static IEnvironment Paper { get; } = new PaperEnvironment();
}
