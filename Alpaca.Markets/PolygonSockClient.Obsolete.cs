using System;
using System.Globalization;

namespace Alpaca.Markets
{
    public sealed partial class PolygonSockClient
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public PolygonSockClient(
            String keyId,
            String? polygonWebsocketApi = null,
            Boolean isStagingEnvironment = false,
            IWebSocketFactory? webSocketFactory = null)
            : this(createConfiguration(
                keyId, polygonWebsocketApi.GetUrlSafe(Environments.Live.PolygonStreamingApi), isStagingEnvironment,  webSocketFactory))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public PolygonSockClient(
            String keyId,
            Uri polygonWebsocketApi,
            Boolean isStagingEnvironment,
            IWebSocketFactory? webSocketFactory)
            : this(createConfiguration(keyId, polygonWebsocketApi, isStagingEnvironment, webSocketFactory))
        {
        }

#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public PolygonSockClient(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null)
            : this(createConfiguration(configuration, webSocketFactory))
        {
            System.Diagnostics.Contracts.Contract.Requires(configuration != null);
        }

        private static PolygonStreamingClientConfiguration createConfiguration(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null) =>
            createConfiguration(
                configuration?["keyId"] ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration["polygonWebsocketApi"].GetUrlSafe(Environments.Live.PolygonStreamingApi),
                Convert.ToBoolean(configuration?["staging"] ?? "false", CultureInfo.InvariantCulture),
                webSocketFactory);
#endif    

        private static PolygonStreamingClientConfiguration createConfiguration(
            String keyId,
            Uri polygonWebsocketApi,
            Boolean isStagingEnvironment,
            IWebSocketFactory? webSocketFactory)
        {
            return new PolygonStreamingClientConfiguration()
            {
                KeyId = adjustKeyId(keyId ?? throw new ArgumentException("Application key id should not be null or empty.", nameof(keyId)), isStagingEnvironment),
                ApiEndpoint = polygonWebsocketApi ?? (isStagingEnvironment ? Environments.Staging.PolygonStreamingApi : Environments.Live.PolygonStreamingApi),
                WebSocketFactory = webSocketFactory ?? WebSocket4NetFactory.Instance
            };
        }

        private static String adjustKeyId(String keyId, Boolean isStagingEnvironment)
        {
            if (isStagingEnvironment &&
                !keyId.EndsWith("-staging", StringComparison.Ordinal))
            {
                keyId += "-staging";
            }
            return keyId;
        }
    }
}
