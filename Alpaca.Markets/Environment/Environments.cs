using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
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

        /// <summary>
        /// Gets environment used by development team for pre-production tests.
        /// </summary>
        public static IEnvironment Staging { get; } = new StagingEnvironment();

        internal static Uri GetUrlSafe(this String? url, Uri defaultUrl) => new Uri(url ?? defaultUrl.AbsoluteUri);
    }
}
