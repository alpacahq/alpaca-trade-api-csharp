using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    [SuppressMessage(
        "Globalization", "CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    [Obsolete("This class is deprecated and will be removed in the upcoming releases. Use the PolygonStreamingClient class instead.", false)]
    public sealed class PolygonSockClient : IDisposable
    {
        private readonly PolygonStreamingClient _client;

        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
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
                Convert.ToBoolean(configuration?["staging"] ?? "false", System.Globalization.CultureInfo.InvariantCulture),
                webSocketFactory);
#endif    

        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        private PolygonSockClient(
            PolygonStreamingClientConfiguration configuration) =>
            _client = new PolygonStreamingClient(configuration);

        /// <summary>
        /// Occured when new trade received from stream.
        /// </summary>
        public event Action<IStreamTrade>? TradeReceived
        {
            add => _client.TradeReceived += value;
            remove => _client.TradeReceived -= value;
        }

        /// <summary>
        /// Occured when new quote received from stream.
        /// </summary>
        public event Action<IStreamQuote>? QuoteReceived
        {
            add => _client.QuoteReceived += value;
            remove => _client.QuoteReceived -= value;
        }

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        public event Action<IStreamAgg>? MinuteAggReceived
        {
            add => _client.MinuteAggReceived += value;
            remove => _client.MinuteAggReceived -= value;
        }

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        public event Action<IStreamAgg>? SecondAggReceived
        {
            add => _client.SecondAggReceived += value;
            remove => _client.SecondAggReceived -= value;
        }

        /// <inheritdoc />
        public void Dispose() => _client.Dispose();

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeTrade(String symbol) => _client.SubscribeTrade(symbol);

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeQuote(String symbol) => _client.SubscribeQuote(symbol);

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeSecondAgg(String symbol) => _client.SubscribeSecondAgg(symbol);

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeMinuteAgg(String symbol) => _client.SubscribeMinuteAgg(symbol);

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeTrade(IEnumerable<String> symbols) => _client.SubscribeTrade(symbols);

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeQuote(IEnumerable<String> symbols) => _client.SubscribeQuote(symbols);

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeSecondAgg(IEnumerable<String> symbols) => _client.SubscribeSecondAgg(symbols);

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeMinuteAgg(IEnumerable<String> symbols) => _client.SubscribeMinuteAgg(symbols);

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeTrade(String symbol) => _client.UnsubscribeTrade(symbol);

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeQuote(String symbol) => _client.UnsubscribeQuote(symbol);

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeSecondAgg(String symbol) => _client.UnsubscribeSecondAgg(symbol);

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeMinuteAgg(String symbol) => _client.UnsubscribeMinuteAgg(symbol);

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeTrade(IEnumerable<String> symbols) => _client.UnsubscribeTrade(symbols);

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeQuote(IEnumerable<String> symbols) => _client.UnsubscribeQuote(symbols);

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeSecondAgg(IEnumerable<String> symbols) => _client.UnsubscribeSecondAgg(symbols);

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeMinuteAgg(IEnumerable<String> symbols) => _client.UnsubscribeMinuteAgg(symbols);

        private static PolygonStreamingClientConfiguration createConfiguration(
            String keyId,
            Uri polygonWebsocketApi,
            Boolean isStagingEnvironment,
            IWebSocketFactory? webSocketFactory)
        {
            return new PolygonStreamingClientConfiguration
            {
                KeyId = adjustKeyId(keyId ?? throw new ArgumentException("Application key id should not be null or empty.", nameof(keyId)), isStagingEnvironment),
                ApiEndpoint = polygonWebsocketApi ?? Environments.Live.PolygonStreamingApi,
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
