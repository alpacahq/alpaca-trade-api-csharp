using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Stores list of base API URLs for different API endpoints available for Alpaca.Markets SDK
    /// on staging environment. This environment used by development team for pre-production tests.
    /// </summary>
    public static class StagingEnvironment
    {
        /// <summary>
        /// Gets Alpaca trading REST API base URL for staging environment.
        /// </summary>
        public static Uri TradingApiUrl { get; } = new Uri("https://staging-api.tradetalk.us");

        /// <summary>
        /// Gets Polygon.io streaming API base URL for staging environment.
        /// </summary>
        public static Uri PolygonApiUrl { get; } = new Uri("wss://alpaca.socket.polygon.io/stocks");
    }
}
