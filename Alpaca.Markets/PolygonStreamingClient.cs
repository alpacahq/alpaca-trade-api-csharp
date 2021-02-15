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

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public PolygonStreamingClient(
            PolygonStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration))) =>
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { StatusMessage, handleAuthorization }
            };

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol) => 
            SubscriptionsGetOrAdd<IStreamTrade, JsonStreamTradeAlpaca>(GetStreamName(TradesChannel, symbol));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol) =>
            SubscriptionsGetOrAdd<IStreamQuote, JsonStreamQuoteAlpaca>(GetStreamName(QuotesChannel, symbol));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() => 
            SubscriptionsWildcardGetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(MinuteAggChannel);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            SubscriptionsGetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(GetStreamName(MinuteAggChannel, symbol));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription() => 
            SubscriptionsWildcardGetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(SecondAggChannel);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetSecondAggSubscription(
            String symbol) =>
            SubscriptionsGetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(GetStreamName(SecondAggChannel, symbol));

        /// <inheritdoc />
        public void Subscribe(
            IAlpacaDataSubscription subscription) =>
            subscribe(subscription.EnsureNotNull(nameof(subscription)).Streams);

        /// <inheritdoc />
        public void Subscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Subscribe(subscriptions.AsEnumerable());

        /// <inheritdoc />
        public void Subscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            subscribe(subscriptions.SelectMany(_ => _.Streams));

        /// <inheritdoc />
        public void Unsubscribe(
            IAlpacaDataSubscription subscription) =>
            unsubscribe(subscription.EnsureNotNull(nameof(subscription)).Streams);

        /// <inheritdoc />
        public void Unsubscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Unsubscribe(subscriptions.AsEnumerable());

        /// <inheritdoc />
        public void Unsubscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            unsubscribe(subscriptions.SelectMany(_ => _.Streams));

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
                        var stream = messageType.ToString();
                        if (handleRealtimeDataUpdate(stream, token))
                        {
                            return;
                        }
                        HandleMessage(_handlers, stream, token);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }
        
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private Boolean handleRealtimeDataUpdate(
            String stream,
            JToken token)
        {
            try
            {
                return SubscriptionsOnReceived(stream, token);
            }
            catch (Exception exception)
            {
                HandleError(exception);
                return false;
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
            IEnumerable<String> streams) =>
            SendAsJsonString(new JsonListenRequest
            {
                Action = JsonAction.PolygonSubscribe,
                Params = getParams(streams)
            });

        private void unsubscribe(
            IEnumerable<String> streams) =>
            SendAsJsonString(new JsonUnsubscribeRequest
            {
                Action = JsonAction.PolygonUnsubscribe,
                Params = getParams(streams)
            });

        private static String getParams(
            IEnumerable<String> streams) =>
            String.Join(",", streams);
    }
}
