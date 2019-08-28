using System;
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

            _handlers = new Dictionary<string, Action<JToken>>(StringComparer.Ordinal)
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
            subscribe(TradesChannel, symbol);

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeQuote(
            String symbol) =>
            subscribe(QuotesChannel, symbol);

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeSecondAgg(
            String symbol) =>
            subscribe(SecondAggChannel, symbol);

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeMinuteAgg(
            String symbol) =>
            subscribe(MinuteAggChannel, symbol);

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeTrade(
            String symbol) =>
            unsubscribe(TradesChannel, symbol);

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeQuote(
            String symbol) =>
            unsubscribe(QuotesChannel, symbol);

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeSecondAgg(
            String symbol) =>
            unsubscribe(SecondAggChannel, symbol);

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            String symbol) =>
            unsubscribe(MinuteAggChannel, symbol);

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

                case ConnectionStatus.Success:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.AuthenticationRequired:
                    HandleError(new InvalidOperationException(connectionStatus.Message));
                    break;

                case ConnectionStatus.AuthenticationFailed:
                    HandleError(new InvalidOperationException(connectionStatus.Message));
                    break;
            }
        }

        private void subscribe(
            String channel,
            String symbol)
        {
            var listenRequest = new JsonListenRequest
            {
                Action = JsonAction.PolygonSubscribe,
                Params = $"{channel}.{symbol}"
            };

            SendAsJsonString(listenRequest);
        }

        private void unsubscribe(
            String channel,
            String symbol)
        {
            var unsubscribeRequest = new JsonUnsubscribeRequest
            {
                Action = JsonAction.PolygonUnsubscribe,
                Params = $"{channel}.{symbol}"
            };

            SendAsJsonString(unsubscribeRequest);
        }

        private void handleTradesChannel(
            JToken token) =>
            TradeReceived.Invoke<IStreamTrade, JsonStreamTrade>(token);

        private void handleQuotesChannel(
            JToken token) =>
            QuoteReceived.Invoke<IStreamQuote, JsonStreamQuote>(token);

        private void handleMinuteAggChannel(
            JToken token) =>
            MinuteAggReceived.Invoke<IStreamAgg, JsonStreamAgg>(token);

        private void handleSecondAggChannel(
            JToken token) =>
            SecondAggReceived.Invoke<IStreamAgg, JsonStreamAgg>(token);
    }
}
