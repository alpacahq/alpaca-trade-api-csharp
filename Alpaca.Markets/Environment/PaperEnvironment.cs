using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Stores list of base API URLs for different API endpoints available for Alpaca.Markets SDK
    /// on paper environment. This environment used by all Alpaca users who have no registered accounts.
    /// </summary>
    public static class PaperEnvironment
    {
        /// <summary>
        /// Gets Alpaca trading REST API base URL for paper environment.
        /// </summary>
        public static Uri TradingApiUrl { get; } = new Uri("https://paper-api.alpaca.markets");
    }
}
