using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="PolygonSockClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class PolygonSockClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClientConfiguration"/> class.
        /// </summary>
        public PolygonSockClientConfiguration()
        {
            KeyId = String.Empty;
            PolygonApiUrl = LiveEnvironment.PolygonApiUrl;
            WebSocketFactory = new WebSocket4NetFactory();
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// Gets or sets Polygon.io streaming API base URL.
        /// </summary>
        public Uri PolygonApiUrl { get; set; }

        /// <summary>
        /// Gets or sets web sockets connection factory.
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
