namespace Alpaca.Markets
{
    /// <summary>
    /// Provides single entry point for obtaining information about different environments.
    /// </summary>
    public static class Environments
    {
        /// <summary>
        /// Gets environment used by all Alpaca users who has fully registered accounts.
        /// </summary>
        public static IEnvironment Live { get; } = new LiveEnvironment();

        /// <summary>
        /// Gets environment used by all Alpaca users who have no registered accounts.
        /// </summary>
        public static IEnvironment Paper { get; } = new PaperEnvironment();
    }
}
