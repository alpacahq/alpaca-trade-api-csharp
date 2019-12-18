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
    public sealed class SockClientConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public SockClientConfiguration()
        {
            KeyId = String.Empty;
            SecretKey = String.Empty;
            TradingApiUrl = LiveEnvironment.TradingApiUrl;
            WebSocketFactory = new WebSocket4NetFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String SecretKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Uri TradingApiUrl { get; set; }

        /// <summary>
        /// 
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

#if !NETSTANDARD2_1
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
#endif

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
