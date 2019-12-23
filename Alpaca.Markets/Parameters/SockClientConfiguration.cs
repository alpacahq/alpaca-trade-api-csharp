using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="SockClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class SockClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="SockClientConfiguration"/> class.
        /// </summary>
        public SockClientConfiguration()
        {
            KeyId = String.Empty;
            SecretKey = String.Empty;
            TradingApiUrl = LiveEnvironment.TradingApiUrl;
            WebSocketFactory = new WebSocket4NetFactory();
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca secret key identifier.
        /// </summary>
        public String SecretKey { get; set; }

        /// <summary>
        /// Gets or sets Alpaca streaming API base URL.
        /// </summary>
        public Uri TradingApiUrl { get; set; }

        /// <summary>
        /// Gets or sets web sockets connection factory.
        /// </summary>
        public IWebSocketFactory WebSocketFactory { get; set; }

        internal SockClientConfiguration EnsureIsValid()
        {
            if (String.IsNullOrEmpty(KeyId))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(KeyId)}' property shouldn't be null or empty.");
            }

            if (String.IsNullOrEmpty(SecretKey))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecretKey)}' property shouldn't be null or empty.");
            }

            if (TradingApiUrl == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(TradingApiUrl)}' property shouldn't be null.");
            }

            if (WebSocketFactory == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(WebSocketFactory)}' property shouldn't be null.");
            }

            return this;
        }

        internal UriBuilder GetUriBuilder()
        {
            var uriBuilder = new UriBuilder(TradingApiUrl)
            {
                Scheme = TradingApiUrl.Scheme == "http" ? "ws" : "wss"
            };

            if (!uriBuilder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                uriBuilder.Path += "/";
            }

            uriBuilder.Path += "stream";
            return uriBuilder;
        }
    }
}
