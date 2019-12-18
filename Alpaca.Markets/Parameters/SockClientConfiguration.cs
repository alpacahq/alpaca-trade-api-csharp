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
            : this(String.Empty, String.Empty, "https://api.alpaca.markets", null)
        {
        }

#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        internal SockClientConfiguration(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null)
            : this(
                configuration?["keyId"]
                    ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration["secretKey"]
                    ?? throw new ArgumentException("Provide 'secretKey' configuration parameter.", nameof(configuration)),
                configuration["alpacaRestApi"]
                    ?? "https://api.alpaca.markets",
                webSocketFactory)
        {
            System.Diagnostics.Contracts.Contract.Requires(configuration != null);
        }
#endif

        /// <summary>
        /// Creates new instance of <see cref="SockClientConfiguration"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        internal SockClientConfiguration(
            String keyId,
            String secretKey,
            String alpacaRestApi,
            IWebSocketFactory? webSocketFactory)
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"),
                webSocketFactory ?? new WebSocket4NetFactory())
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClientConfiguration"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        internal SockClientConfiguration(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            IWebSocketFactory? webSocketFactory)
        {
            KeyId = keyId ?? throw new ArgumentException(
                "Application key id should not be null", nameof(keyId));
            SecretKey = secretKey ?? throw new ArgumentException(
                            "Application secret key should not be null", nameof(secretKey));

            AlpacaRestApi = alpacaRestApi;
            WebSocketFactory = webSocketFactory ?? new WebSocket4NetFactory();
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
        public Uri AlpacaRestApi { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IWebSocketFactory WebSocketFactory { get; set; }

        internal void EnsureIsValid()
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
        }
    }
}
