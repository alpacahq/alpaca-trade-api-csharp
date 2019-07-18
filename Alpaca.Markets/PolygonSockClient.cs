using System;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    public sealed partial class PolygonSockClient : SockClientBase
    {
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
        /// Occured when any error happened in stream.
        /// </summary>
        public event Action<Exception> OnError;

        /// <summary>
        /// Creates new instance of <see cref="PolygonSockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public PolygonSockClient(
            String keyId,
            String polygonWebsocketApi = null,
            IWebSocketFactory webSocketFactory = null
            )
            : this(
                keyId,
                new Uri(polygonWebsocketApi ?? "wss://alpaca.socket.polygon.io/stocks"),
                webSocketFactory ?? new WebSocket4NetFactory())
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        public PolygonSockClient(
            String keyId,
            Uri polygonWebsocketApi,
            IWebSocketFactory webSocketFactory)
        {
            _keyId = keyId ?? throw new ArgumentException(nameof(keyId));

            polygonWebsocketApi = polygonWebsocketApi ?? new Uri("wss://alpaca.socket.polygon.io/stocks");

            var uriBuilder = new UriBuilder(polygonWebsocketApi)
            {
                Scheme = polygonWebsocketApi.Scheme == "http" ? "ws" : "wss"
            };

            setupWebSocket(uriBuilder, webSocketFactory);
            _webSocket.Error += handleError;
            _webSocket.MessageReceived += handleMessageReceived;
        }

        override internal JsonAuthRequest getAuthRequest()
        {
            return new JsonAuthRequest
            {
                Action = JsonAction.PolygonAuthenticate,
                Params = _keyId
            };
        }

        private void handleError(Exception exception)
        {
            OnError?.Invoke(exception);
        }

        private void handleMessageReceived(String message)
        {
            try
            {
                var rootArray = JArray.Parse(message);

                foreach (var root in rootArray)
                {
                    var stream = root["ev"].ToString();
                    switch (stream)
                    {
                        case "T":
                            TradeReceived?.Invoke(root.ToObject<JsonStreamTrade>());
                            break;
                        case "Q":
                            QuoteReceived?.Invoke(root.ToObject<JsonStreamQuote>());
                            break;
                        case "AM":
                            MinuteAggReceived?.Invoke(root.ToObject<JsonStreamAgg>());
                            break;
                        case "A":
                            SecondAggReceived?.Invoke(root.ToObject<JsonStreamAgg>());
                            break;
                        default:
                            OnError?.Invoke(new InvalidOperationException(
                                $"Unexpected message type '{stream}' received."));
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                OnError?.Invoke(exception);
            }
        }

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeTrade(
            String symbol)
        {
            subscribe("T", symbol);
        }

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeQuote(
            String symbol)
        {
            subscribe("Q", symbol);
        }

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeSecondAgg(
            String symbol)
        {
            subscribe("A", symbol);
        }

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeMinuteAgg(
            String symbol)
        {
            subscribe("AM", symbol);
        }

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeTrade(
            String symbol)
        {
            unsubscribe("T", symbol);
        }

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeQuote(
            String symbol)
        {
            unsubscribe("Q", symbol);
        }

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeSecondAgg(
            String symbol)
        {
            unsubscribe("A", symbol);
        }

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            String symbol)
        {
            unsubscribe("AM", symbol);
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

            sendAsJsonString(listenRequest);
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

            sendAsJsonString(unsubscribeRequest);
        }
    }
}
