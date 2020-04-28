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
        private interface ISubscription
        {
            void OnUpdate(
                Boolean state);

            void OnReceived(
                JToken token);
        }

        private sealed class AlpacaDataSubscription<TApi, TJson> : IAlpacaDataSubscription<TApi>, ISubscription
            where TJson : class, TApi
            where TApi : IStreamBase
        {
            internal AlpacaDataSubscription(
                String stream) =>
                Stream = stream;

            public String Stream { get; }

            public Boolean Subscribed { get; private set; }

            public event Action<TApi>? Received;

            public void OnReceived(
                JToken token) =>
                Received?.Invoke(token.ToObject<TJson>() ??
                                 throw new RestClientErrorException());

            public void OnUpdate(
                Boolean state) => Subscribed = state;
        }

        // Available Alpaca data streaming message types

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String Listening = "listening";

        private const String Authorization = "authorization";

        private readonly ConcurrentDictionary<String, ISubscription> _subscriptions =
            new ConcurrentDictionary<String, ISubscription>(StringComparer.Ordinal);

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
                { Authorization, handleAuthorization }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol) =>
            getOrAdd<IStreamTrade, JsonStreamTrade>(getStreamName(TradesChannel, symbol));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol) =>
            getOrAdd<IStreamQuote, JsonStreamQuote>(getStreamName(QuotesChannel, symbol));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            getOrAdd<IStreamAgg, JsonStreamAgg>(getStreamName(MinuteAggChannel, symbol));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscription"></param>
        public void Subscribe(
            IAlpacaDataSubscription subscription) =>
            subscribe(new []
            {
                subscription.EnsureNotNull(nameof(subscription)).Stream
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptions"></param>
        public void Subscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Subscribe(subscriptions.AsEnumerable());

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
        /// <param name="subscription"></param>
        public void Unsubscribe(
            IAlpacaDataSubscription subscription) =>
            unsubscribe(new []
            {
                subscription.EnsureNotNull(nameof(subscription)).Stream
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptions"></param>
        public void Unsubscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Unsubscribe(subscriptions.AsEnumerable());

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
                var token = JObject.Parse(Encoding.UTF8.GetString(binaryData));

                var payload = token["data"];
                var stream = token["stream"];

                if (payload is null ||
                    stream is null)
                {
                    HandleError(new InvalidOperationException());
                }
                else
                {
                    var streamAsString = stream.ToString();
                    if (handleRealtimeDataUpdate(streamAsString, payload))
                    {
                        return;
                    }
                    HandleMessage(_handlers, streamAsString, payload);
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
                handleListeningStateUpdate(symbol, true);
            }
        }

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private bool handleRealtimeDataUpdate(
            String stream,
            JToken token)
        {
            if (!_subscriptions.TryGetValue(stream, out var subscription))
            {
                return false;
            }

            try
            {
                subscription.OnReceived(token);
                return true;
            }
            catch (Exception exception)
            {
                HandleError(exception);
                return false;
            }
        }

        private void handleListeningStateUpdate(
            String stream,
            Boolean state)
        {
            if (_subscriptions.TryGetValue(stream, out var subscription))
            {
                subscription.OnUpdate(state);
            }
        }
        
        private  IAlpacaDataSubscription<TApi> getOrAdd<TApi, TJson>(
            String stream)
            where TJson : class, TApi
            where TApi : IStreamBase =>
            (IAlpacaDataSubscription<TApi>) _subscriptions.GetOrAdd(
                stream, (key) => new AlpacaDataSubscription<TApi, TJson>(key));

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

        private static String getStreamName(
            String channelName,
            String symbol) =>
            $"{channelName}.{symbol}";
    }
}
