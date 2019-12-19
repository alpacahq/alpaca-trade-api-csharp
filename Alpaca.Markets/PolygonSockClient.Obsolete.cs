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
            : this(createConfiguration(keyId, new Uri(polygonWebsocketApi ?? LiveEnvironment.PolygonApiUrl.AbsoluteUri), isStagingEnvironment,  webSocketFactory))
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

        private static PolygonSockClientConfiguration createConfiguration(
            Microsoft.Extensions.Configuration.IConfiguration configuration,
            IWebSocketFactory? webSocketFactory = null) =>
            createConfiguration(
                configuration?["keyId"] ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                new Uri(configuration?["polygonWebsocketApi"] ?? LiveEnvironment.PolygonApiUrl.AbsoluteUri),
                Convert.ToBoolean(configuration?["staging"] ?? "false", CultureInfo.InvariantCulture),
                webSocketFactory);
#endif    

        private static PolygonSockClientConfiguration createConfiguration(
            String keyId,
            Uri polygonWebsocketApi,
            Boolean isStagingEnvironment,
            IWebSocketFactory? webSocketFactory)
        {
            return new PolygonSockClientConfiguration()
            {
                KeyId = adjustKeyId(keyId ?? throw new ArgumentException("Application key id should not be null or empty.", nameof(keyId)), isStagingEnvironment),
                PolygonApiUrl = polygonWebsocketApi ?? (isStagingEnvironment ? StagingEnvironment.PolygonApiUrl : LiveEnvironment.PolygonApiUrl),
                WebSocketFactory = webSocketFactory ?? new WebSocket4NetFactory()
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
