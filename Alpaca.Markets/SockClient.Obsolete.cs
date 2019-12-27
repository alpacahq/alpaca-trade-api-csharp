using System;

namespace Alpaca.Markets
{
    public sealed partial class SockClient
    {
        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            String keyId,
            String secretKey,
            String? alpacaRestApi = null,
            IWebSocketFactory? webSocketFactory = null)
            : this(createConfiguration(
                keyId, secretKey, alpacaRestApi.GetUrlSafe(Environments.Live.AlpacaTradingApi), webSocketFactory))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            IWebSocketFactory? webSocketFactory)
            : this(createConfiguration(keyId, secretKey, alpacaRestApi, webSocketFactory))
        {
        }

#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public SockClient(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null)
            : this(createConfiguration(configuration, webSocketFactory)) =>
            System.Diagnostics.Contracts.Contract.Requires(configuration != null);

        private static AlpacaStreamingClientConfiguration createConfiguration(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null) =>
            createConfiguration(
                configuration?["keyId"] ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration["secretKey"] ?? throw new ArgumentException("Provide 'secretKey' configuration parameter.", nameof(configuration)),
                configuration["alpacaRestApi"].GetUrlSafe(Environments.Live.AlpacaTradingApi),
                webSocketFactory);
#endif

        private static AlpacaStreamingClientConfiguration createConfiguration(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            IWebSocketFactory? webSocketFactory) =>
            new AlpacaStreamingClientConfiguration
            {
                KeyId = keyId ?? throw new ArgumentException("Application key id should not be null.", nameof(keyId)),
                SecretKey = secretKey ?? throw new ArgumentException("Application secret key should not be null.", nameof(secretKey)),
                ApiEndpoint = alpacaRestApi ?? Environments.Live.AlpacaTradingApi,
                WebSocketFactory = webSocketFactory ?? WebSocket4NetFactory.Instance,
            };
    }
}
