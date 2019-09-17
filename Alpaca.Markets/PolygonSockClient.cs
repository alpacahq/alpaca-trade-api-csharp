using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    // ReSharper disable once PartialTypeWithSinglePart
    public sealed partial class PolygonSockClient : SockClientBase
    {
        // Available Polygon message types

        // ReSharper disable InconsistentNaming

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String SecondAggChannel = "A";

        private const String StatusMessage = "status";

        // ReSharper restore InconsistentNaming

        private readonly IDictionary<String, Action<JToken>> _handlers;

        private readonly String _keyId;

        /// <summary>
        /// Occured when new trade received from stream.
        /// </summary>
        public event Action<IStreamTrade> TradeReceived;

        /// <summary>
        /// Occured when new quote received from stream.
        /// </summary>
        public event Action<IStreamQuote> QuoteReceived;

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        public event Action<IStreamAgg> MinuteAggReceived;

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        public event Action<IStreamAgg> SecondAggReceived;

        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public PolygonSockClient(
            String keyId,
            String polygonWebsocketApi = null,
            Boolean isStagingEnvironment = false,
            IWebSocketFactory webSocketFactory = null
            )
            : this(
                keyId,
                new Uri(polygonWebsocketApi ?? "wss://alpaca.socket.polygon.io/stocks"),
                isStagingEnvironment,
                webSocketFactory ?? new WebSocket4NetFactory())
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
            IWebSocketFactory webSocketFactory)
            : base(getUriBuilder(polygonWebsocketApi), webSocketFactory)
        {
            _keyId = keyId ?? throw new ArgumentException(
                         "Application key id should not be null", nameof(keyId));

            if (isStagingEnvironment &&
                !_keyId.EndsWith("-staging", StringComparison.Ordinal))
            {
                _keyId += "-staging";
            }

            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { StatusMessage, handleAuthorization },
                { TradesChannel, handleTradesChannel },
                { QuotesChannel, handleQuotesChannel },
                { MinuteAggChannel, handleMinuteAggChannel },
                { SecondAggChannel, handleSecondAggChannel }
            };
        }

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeTrade(
            String symbol) =>
            subscribe(getParams(TradesChannel, symbol));

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeQuote(
            String symbol) =>
            subscribe(getParams(QuotesChannel, symbol));

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeSecondAgg(
            String symbol) =>
            subscribe(getParams(SecondAggChannel, symbol));

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeMinuteAgg(
            String symbol) =>
            subscribe(getParams(MinuteAggChannel, symbol));

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeTrade(
            IEnumerable<String> symbols) =>
            subscribe(getParams(TradesChannel, symbols));

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeQuote(
            IEnumerable<String> symbols) =>
            subscribe(getParams(QuotesChannel, symbols));

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeSecondAgg(
            IEnumerable<String> symbols) =>
            subscribe(getParams(SecondAggChannel, symbols));

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void SubscribeMinuteAgg(
            IEnumerable<String> symbols) =>
            subscribe(getParams(MinuteAggChannel, symbols));

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeTrade(
            String symbol) =>
            unsubscribe(getParams(TradesChannel, symbol));

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeQuote(
            String symbol) =>
            unsubscribe(getParams(QuotesChannel, symbol));

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeSecondAgg(
            String symbol) =>
            unsubscribe(getParams(SecondAggChannel, symbol));

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            String symbol) =>
            unsubscribe(getParams(MinuteAggChannel, symbol));

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeTrade(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(TradesChannel, symbols));

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeQuote(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(QuotesChannel, symbols));

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeSecondAgg(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(SecondAggChannel, symbols));

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(MinuteAggChannel, symbols));

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                foreach (var token in JArray.Parse(message))
                {
                    HandleMessage(_handlers, token["ev"].ToString(), token);
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private static UriBuilder getUriBuilder(
            Uri polygonWebsocketApi)
        {
            polygonWebsocketApi = polygonWebsocketApi ?? new Uri("wss://alpaca.socket.polygon.io/stocks");

            var uriBuilder = new UriBuilder(polygonWebsocketApi)
            {
                Scheme = polygonWebsocketApi.Scheme == "http" ? "ws" : "wss"
            };
            return uriBuilder;
        }

        private void handleAuthorization(
            JToken token)
        {
            var connectionStatus = token.ToObject<JsonConnectionStatus>();

            switch (connectionStatus.Status)
            {
                case ConnectionStatus.Connected:
                    SendAsJsonString(new JsonAuthRequest
                    {
                        Action = JsonAction.PolygonAuthenticate,
                        Params = _keyId
                    });
                    break;

                case ConnectionStatus.AuthenticationSuccess:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.Failed:
                case ConnectionStatus.AuthenticationFailed:
                case ConnectionStatus.AuthenticationRequired:
                    HandleError(new InvalidOperationException(connectionStatus.Message));
                    break;

                default:
                    // Just ignore other statuses for now
                    break;
            }
        }

        private void subscribe(
            String parameters) =>
            SendAsJsonString(new JsonListenRequest
            {
                Action = JsonAction.PolygonSubscribe,
                Params = parameters
            });

        private void unsubscribe(
            String parameters) =>
            SendAsJsonString(new JsonUnsubscribeRequest
            {
                Action = JsonAction.PolygonUnsubscribe,
                Params = parameters
            });

        private static String getParams(
            String channel,
            String symbol) =>
            $"{channel}.{symbol}";

        private static String getParams(
            String channel,
            IEnumerable<String> symbols) =>
            String.Join(",",symbols.Select(symbol => getParams(channel, symbol)));

        private void handleTradesChannel(
            JToken token) =>
            TradeReceived.DeserializeAndInvoke<IStreamTrade, JsonStreamTrade>(token);

        private void handleQuotesChannel(
            JToken token) =>
            QuoteReceived.DeserializeAndInvoke<IStreamQuote, JsonStreamQuote>(token);

        private void handleMinuteAggChannel(
            JToken token) =>
            MinuteAggReceived.DeserializeAndInvoke<IStreamAgg, JsonStreamAgg>(token);

        private void handleSecondAggChannel(
            JToken token) =>
            SecondAggReceived.DeserializeAndInvoke<IStreamAgg, JsonStreamAgg>(token);
    }
}
