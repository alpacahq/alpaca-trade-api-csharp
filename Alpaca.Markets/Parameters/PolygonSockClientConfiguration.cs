using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class PolygonSockClientConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public PolygonSockClientConfiguration()
        {
            KeyId = String.Empty;
            PolygonApiUrl = LiveEnvironment.PolygonApiUrl;
            WebSocketFactory = new WebSocket4NetFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri PolygonApiUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IWebSocketFactory WebSocketFactory { get; set; }

        
        internal PolygonSockClientConfiguration EnsureIsValid()
        {
            if (String.IsNullOrEmpty(KeyId))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(KeyId)}' property shouldn't be null or empty.");
            }

            if (PolygonApiUrl == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(PolygonApiUrl)}' property shouldn't be null.");
            }

            if (WebSocketFactory == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(WebSocketFactory)}' property shouldn't be null.");
            }

            return this;
        }

        internal UriBuilder GetUriBuilder() =>
            new UriBuilder(PolygonApiUrl)
            {
                Scheme = PolygonApiUrl.Scheme == "http" ? "ws" : "wss"
            };
    }
}
