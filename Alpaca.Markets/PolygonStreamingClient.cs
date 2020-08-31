using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    public sealed class PolygonStreamingClient :
        StreamingClientBase<PolygonStreamingClientConfiguration>,
        IPolygonStreamingClient
    {
        // Available Polygon message types

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String SecondAggChannel = "A";

        private const String StatusMessage = "status";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        /// <inheritdoc />
        public event Action<IStreamTrade>? TradeReceived;

        /// <inheritdoc />
        public event Action<IStreamQuote>? QuoteReceived;

        /// <inheritdoc />
        public event Action<IStreamAgg>? MinuteAggReceived;

        /// <inheritdoc />
        public event Action<IStreamAgg>? SecondAggReceived;

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public PolygonStreamingClient(
            PolygonStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { StatusMessage, handleAuthorization },
                { TradesChannel, handleTradesChannel },
                { QuotesChannel, handleQuotesChannel },
                { MinuteAggChannel, handleMinuteAggChannel },
                { SecondAggChannel, handleSecondAggChannel }
            };
        }

        /// <inheritdoc />
        public void SubscribeTrade(
            String symbol) =>
            subscribe(getParams(TradesChannel, symbol));

        /// <inheritdoc />
        public void SubscribeQuote(
            String symbol) =>
            subscribe(getParams(QuotesChannel, symbol));

        /// <inheritdoc />
        public void SubscribeSecondAgg(
            String symbol) =>
            subscribe(getParams(SecondAggChannel, symbol));

        /// <inheritdoc />
        public void SubscribeMinuteAgg(
            String symbol) =>
            subscribe(getParams(MinuteAggChannel, symbol));

        /// <inheritdoc />
        public void SubscribeTrade(
            IEnumerable<String> symbols) =>
            subscribe(getParams(TradesChannel, symbols));

        /// <inheritdoc />
        public void SubscribeQuote(
            IEnumerable<String> symbols) =>
            subscribe(getParams(QuotesChannel, symbols));

        /// <inheritdoc />
        public void SubscribeSecondAgg(
            IEnumerable<String> symbols) =>
            subscribe(getParams(SecondAggChannel, symbols));

        /// <inheritdoc />
        public void SubscribeMinuteAgg(
            IEnumerable<String> symbols) =>
            subscribe(getParams(MinuteAggChannel, symbols));

        /// <inheritdoc />
        public void UnsubscribeTrade(
            String symbol) =>
            unsubscribe(getParams(TradesChannel, symbol));

        /// <inheritdoc />
        public void UnsubscribeQuote(
            String symbol) =>
            unsubscribe(getParams(QuotesChannel, symbol));

        /// <inheritdoc />
        public void UnsubscribeSecondAgg(
            String symbol) =>
            unsubscribe(getParams(SecondAggChannel, symbol));

        /// <inheritdoc />
        public void UnsubscribeMinuteAgg(
            String symbol) =>
            unsubscribe(getParams(MinuteAggChannel, symbol));

        /// <inheritdoc />
        public void UnsubscribeTrade(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(TradesChannel, symbols));

        /// <inheritdoc />
        public void UnsubscribeQuote(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(QuotesChannel, symbols));

        /// <inheritdoc />
        public void UnsubscribeSecondAgg(
            IEnumerable<String> symbols) =>
            unsubscribe(getParams(SecondAggChannel, symbols));

        /// <inheritdoc />
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
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private void handleAuthorization(
            JToken token)
        {
            var connectionStatus = token.ToObject<JsonConnectionStatus>();

            // ReSharper disable once ConstantConditionalAccessQualifier
            switch (connectionStatus?.Status)
            {
                case ConnectionStatus.Connected:
                    SendAsJsonString(new JsonAuthRequest
                    {
                        Action = JsonAction.PolygonAuthenticate,
                        Params = Configuration.KeyId
                    });
                    break;

                case ConnectionStatus.AuthenticationSuccess:
                    OnConnected(AuthStatus.Authorized);
                    break;

                case ConnectionStatus.AuthenticationFailed:
                case ConnectionStatus.AuthenticationRequired:
                    HandleError(new InvalidOperationException(connectionStatus.Message));
                    break;

                case ConnectionStatus.Failed:
                case ConnectionStatus.Success:
                    break;

                default:
                    HandleError(new InvalidOperationException("Unknown connection status"));
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
            TradeReceived.DeserializeAndInvoke<IStreamTrade, JsonStreamTradePolygon>(token);

        private void handleQuotesChannel(
            JToken token) =>
            QuoteReceived.DeserializeAndInvoke<IStreamQuote, JsonStreamQuotePolygon>(token);

        private void handleMinuteAggChannel(
            JToken token) =>
            MinuteAggReceived.DeserializeAndInvoke<IStreamAgg, JsonStreamAggPolygon>(token);

        private void handleSecondAggChannel(
            JToken token) =>
            SecondAggReceived.DeserializeAndInvoke<IStreamAgg, JsonStreamAggPolygon>(token);
    }
}
