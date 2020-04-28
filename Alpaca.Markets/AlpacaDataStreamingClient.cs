using System;
using System.Collections.Concurrent;
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
        private sealed class Subscriptions<TApi, TJson>
            where TJson : class, TApi
            where TApi : IStreamBase
        {
            private readonly ConcurrentDictionary<String, AlpacaDataSubscription<TApi>> _items =
                new ConcurrentDictionary<String, AlpacaDataSubscription<TApi>>(StringComparer.Ordinal);

            private readonly String _channelName;

            public Subscriptions(
                String channelName)
            {
                _channelName = channelName;
            }

            public IAlpacaDataSubscription<TApi> GetOrAdd(
                String symbol) =>
                _items.GetOrAdd(symbol, createSubscription);

            public void HandleUpdate(
                JToken token)
            {
                var update = token.ToObject<TJson>()
                             ?? throw new RestClientErrorException("Unable to deserialize JSON response message.");

                _items.TryGetValue(update.Symbol, out var subscription);
                subscription.OnReceived(update);
            }

            public void HandleUpdate(
                String symbol)
            {
                if (_items.TryGetValue(symbol, out var subscription))
                {
                    subscription.OnUpdate();
                }
            }
                
            private AlpacaDataSubscription<TApi> createSubscription(
                String symbol) =>
                new AlpacaDataSubscription<TApi>(_channelName, symbol);
        }

        // Available Alpaca data streaming message types

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String Listening = "listening";

        private const String Authorization = "authorization";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        private readonly Subscriptions<IStreamTrade, JsonStreamTrade> _tradeSubscriptions = 
            new Subscriptions<IStreamTrade, JsonStreamTrade>(TradesChannel);

        private readonly Subscriptions<IStreamQuote, JsonStreamQuote> _quoteSubscriptions = 
            new Subscriptions<IStreamQuote, JsonStreamQuote>(QuotesChannel);

        private readonly Subscriptions<IStreamAgg, JsonStreamAgg> _minuteAggSubscriptions = 
            new Subscriptions<IStreamAgg, JsonStreamAgg>(MinuteAggChannel);

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
                { TradesChannel, _tradeSubscriptions.HandleUpdate },
                { QuotesChannel, _quoteSubscriptions.HandleUpdate },
                { MinuteAggChannel, _minuteAggSubscriptions.HandleUpdate }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol) =>
            _tradeSubscriptions.GetOrAdd(symbol);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol) =>
            _quoteSubscriptions.GetOrAdd(symbol);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            _minuteAggSubscriptions.GetOrAdd(symbol);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptions"></param>
        public void Subscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            subscribe(subscriptions.Select(_ => _.Stream));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptions"></param>
        public void Unsubscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            unsubscribe(subscriptions.Select(_ => _.Stream));

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

            foreach (var symbol in listeningUpdate.Streams)
            {
                _tradeSubscriptions.HandleUpdate(symbol);
                _quoteSubscriptions.HandleUpdate(symbol);
                _minuteAggSubscriptions.HandleUpdate(symbol);
            }
        }

        private void subscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(streams, JsonAction.Listen);

        private void unsubscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(streams, JsonAction.Unlisten);

        private void sendSubscriptionRequest(
            IEnumerable<String> streams,
            JsonAction action) =>
            SendAsJsonString(new JsonListenRequest
            {
                Action = action,
                Data = new JsonListenRequest.JsonData
                {
                    Streams = streams.ToList()
                }
            });
    }
}
