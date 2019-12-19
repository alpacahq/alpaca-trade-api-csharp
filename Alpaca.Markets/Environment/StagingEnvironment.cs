using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public static class StagingEnvironment
    {
        /// <summary>
        /// 
        /// </summary>
        public static Uri TradingApiUrl { get; } = new Uri("https://staging-api.tradetalk.us");

        /// <summary>
        /// 
        /// </summary>
        public static Uri PolygonApiUrl { get; } = new Uri("wss://alpaca.socket.polygon.io/stocks");
    }
}
