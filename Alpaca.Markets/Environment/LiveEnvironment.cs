using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Stores list of base API URLs for different API endpoints available for Alpaca.Markets SDK
    /// on live environment. This environment used by all Alpaca users who has fully registered accounts.
    /// </summary>
    public static class LiveEnvironment
    {
        /// <summary>
        /// Gets Alpaca trading REST API base URL for live environment.
        /// </summary>
        public static Uri TradingApiUrl { get; } = new Uri("https://api.alpaca.markets");

        /// <summary>
        /// Gets Alpaca data REST API base URL for live environment.
        /// </summary>
        public static Uri DataApiUrl { get; } = new Uri("https://data.alpaca.markets");

        /// <summary>
        /// Gets Polygon.io streaming API base URL for live environment.
        /// </summary>
        public static Uri PolygonApiUrl { get; } = new Uri("wss://alpaca.socket.polygon.io/stocks");

        /// <summary>
        /// Gets Polygon.io data REST API base URL for live environment.
        /// </summary>
        public static Uri PolygonRestApi { get; } = new Uri("https://api.polygon.io");
    }
}
