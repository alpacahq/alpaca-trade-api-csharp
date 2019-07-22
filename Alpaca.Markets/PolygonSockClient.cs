using System;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    // ReSharper disable once PartialTypeWithSinglePart
    public sealed partial class PolygonSockClient : SockClientBase
    {
        // Available Polygon channels

        // ReSharper disable InconsistentNaming

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String SecondAggChannel = "A";

        // ReSharper restore InconsistentNaming

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
            : base(getUriBuilder(polygonWebsocketApi), webSocketFactory) =>
            _keyId = keyId ?? throw new ArgumentException(nameof(keyId));

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

        internal override JsonAuthRequest GetAuthRequest() =>
            new JsonAuthRequest
            {
                Action = JsonAction.PolygonAuthenticate,
                Params = _keyId
            };

        /// <inheritdoc/>
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                var rootArray = JArray.Parse(message);

                foreach (var root in rootArray)
                {
                    var stream = root["ev"].ToString();

                    switch (stream)
                    {
                        case TradesChannel:
                            TradeReceived?.Invoke(root.ToObject<JsonStreamTrade>());
                            break;

                        case QuotesChannel:
                            QuoteReceived?.Invoke(root.ToObject<JsonStreamQuote>());
                            break;

                        case MinuteAggChannel:
                            MinuteAggReceived?.Invoke(root.ToObject<JsonStreamAgg>());
                            break;

                        case SecondAggChannel:
                            SecondAggReceived?.Invoke(root.ToObject<JsonStreamAgg>());
                            break;

                        default:
                            HandleError(new InvalidOperationException(
                                $"Unexpected message type '{stream}' received."));
                            break;
                    }
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
    }
}
