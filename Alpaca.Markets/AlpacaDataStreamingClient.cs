using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    public sealed class AlpacaDataStreamingClient : StreamingClientBase<AlpacaDataStreamingClientConfiguration>
    {
        // Available Polygon message types

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String Listening = "listening";

        private const String Authorization = "authorization";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaDataStreamingClient(
            AlpacaDataStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { Listening, handleListeningUpdates },
                { Authorization, handleAuthorization },
                { TradesChannel, handleTradesChannel },
                { QuotesChannel, handleQuotesChannel },
                { MinuteAggChannel, handleMinuteAggChannel }
            };
        }

        /// <summary>
        /// Occured when new trade received from stream.
        /// </summary>
        public event Action<IStreamTrade>? TradeReceived;

        /// <summary>
        /// Occured when new quote received from stream.
        /// </summary>
        public event Action<IStreamQuote>? QuoteReceived;

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        public event Action<IStreamAgg>? MinuteAggReceived;

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
        /// Unsubscribes from the minute bar updates via <see cref="MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        public void UnsubscribeMinuteAgg(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(MinuteAggChannel, symbols));
        
        /// <inheritdoc/>
        protected override void OnOpened()
        {
            SendAsJsonString(new JsonAuthRequest
            {
                Action = JsonAction.Authenticate,
                Data = Configuration.SecurityId
                    .GetAuthenticationData()
            });

            base.OnOpened();
        }

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnDataReceived(
            Byte[] binaryData)
        {
            try
            {
                var token = JToken.Parse(Encoding.UTF8.GetString(binaryData));

                switch (token)
                {
                    case JArray parsedArray:
                        handleRealtimeData(parsedArray);
                        break;

                    case JObject parsedObject:
                        handleSingleObject(parsedObject);
                        break;

                    default:
                        HandleError(new InvalidOperationException());
                        break;
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        /// <inheritdoc/>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                var token = JToken.Parse(message);

                switch (token)
                {
                    case JArray parsedArray:
                        handleRealtimeData(parsedArray);
                        break;

                    case JObject parsedObject:
                        handleSingleObject(parsedObject);
                        break;

                    default:
                        HandleError(new InvalidOperationException());
                        break;
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private void handleSingleObject(
            JObject parsedObject)
        {
            var payload = parsedObject["data"];
            var messageType = parsedObject["stream"];

            if (payload is null ||
                messageType is null)
            {
                HandleError(new InvalidOperationException());
            }
            else
            {
                HandleMessage(_handlers, messageType.ToString(), payload);
            }
        }

        private void handleRealtimeData(
            JArray parsedArray)
        {
            foreach (var token in parsedArray)
            {
                var messageType = token["ev"];
                if (messageType is null)
                {
                    HandleError(new InvalidOperationException());
                }
                else
                {
                    HandleMessage(_handlers, messageType.ToString(), token);
                }
            }
        }

        private void handleAuthorization(
            JToken token)
        {
            var connectionStatus = token.ToObject<JsonConnectionStatus>() ?? new JsonConnectionStatus();

            // ReSharper disable once ConstantConditionalAccessQualifier
            switch (connectionStatus.Status)
            {
                case ConnectionStatus.AlpacaDataStreamingAuthorized:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.AlpacaDataStreamingUnauthorized:
                    OnConnected(AuthStatus.Unauthorized);
                    break;

                default:
                    HandleError(new InvalidOperationException("Unknown connection status"));
                    break;
            }
        }
        
        private void handleListeningUpdates(
            JToken token)
        {
            var listeningUpdate = token.ToObject<JsonListeningUpdate>() ?? new JsonListeningUpdate();

            if (!String.IsNullOrEmpty(listeningUpdate.Error))
            {
                HandleError(new InvalidOperationException(listeningUpdate.Error));
            }

            foreach (var _ in listeningUpdate.Streams)
            {
                // TODO: olegra - update subscription status for assets
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
    }
}
