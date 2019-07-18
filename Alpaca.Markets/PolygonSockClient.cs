using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using System.Security.Authentication;

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
        public PolygonSockClient(
            String keyId,
            String polygonWebsocketApi = null)
            : this(
                keyId,
                new Uri(polygonWebsocketApi ?? "wss://alpaca.socket.polygon.io/stocks"))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="SockClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="polygonWebsocketApi">Polygon websocket API endpoint URL.</param>
        public PolygonSockClient(
            String keyId,
            Uri polygonWebsocketApi)
        {
            _keyId = keyId ?? throw new ArgumentException(nameof(keyId));

            polygonWebsocketApi = polygonWebsocketApi ?? new Uri("wss://alpaca.socket.polygon.io/stocks");

            var uriBuilder = new UriBuilder(polygonWebsocketApi)
            {
                Scheme = polygonWebsocketApi.Scheme == "http" ? "ws" : "wss"
            };

            setupWebSocket(uriBuilder.Uri.ToString());
            _webSocket.Error += (sender, args) => OnError?.Invoke(args.Exception);
            _webSocket.MessageReceived += handleMessageReceived;
        }

        override private protected JsonAuthRequest getAuthRequest()
        {
            return new JsonAuthRequest
            {
                Action = JsonAction.PolygonAuthenticate,
                Params = _keyId
            };
        }

        private void handleMessageReceived(
            Object sender,
            MessageReceivedEventArgs e)
        {
            try
            {
                var rootArray = JArray.Parse(e.Message);

                foreach (JObject root in rootArray)
                {
                    var stream = root["ev"].ToString();
                    switch (stream)
                    {
                        case "T":
                            handleTradeReceived(root.ToObject<JsonStreamTrade>());
                            break;
                        case "Q":
                            handleQuoteReceived(root.ToObject<JsonStreamQuote>());
                            break;
                        case "AM":
                            handleMinuteAggReceived(root.ToObject<JsonStreamAgg>());
                            break;
                        case "A":
                            handleSecondAggReceived(root.ToObject<JsonStreamAgg>());
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

        private void handleTradeReceived(
            IStreamTrade update)
        {
            TradeReceived?.Invoke(update);
        }

        private void handleQuoteReceived(
            IStreamQuote update)
        {
            QuoteReceived?.Invoke(update);
        }

        private void handleMinuteAggReceived(
            IStreamAgg update)
        {
            MinuteAggReceived?.Invoke(update);
        }

        private void handleSecondAggReceived(
            IStreamAgg update)
        {
            SecondAggReceived?.Invoke(update);
        }

        /// <summary>
        /// Subscribes for the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeTrade(
            String symbol)
        {
            subscribe($"T.{symbol}");
        }

        /// <summary>
        /// Subscribes for the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeQuote(
            String symbol)
        {
            subscribe($"Q.{symbol}");
        }

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeSecondAgg(
            String symbol)
        {
            subscribe($"A.{symbol}");
        }

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void SubscribeMinuteAgg(
            String symbol)
        {
            subscribe($"AM.{symbol}");
        }

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeTrade(
            String symbol)
        {
            unsubscribe($"T.{symbol}");
        }

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeQuote(
            String symbol)
        {
            unsubscribe($"Q.{symbol}");
        }

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeSecondAgg(
            String symbol)
        {
            unsubscribe($"A.{symbol}");
        }

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            String symbol)
        {
            unsubscribe($"AM.{symbol}");
        }

        private void subscribe(
            String topic)
        {
            var listenRequest = new JsonListenRequest
            {
                Action = JsonAction.PolygonSubscribe,
                Params = topic
            };

            sendAsJsonString(listenRequest);
        }

        private void unsubscribe(
            String topic)
        {
            var listenRequest = new JsonUnsubscribeRequest
            {
                Action = JsonAction.PolygonUnsubscribe,
                Params = topic
            };

            sendAsJsonString(listenRequest);
        }
    }
}
