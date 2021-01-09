using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    public sealed class AlpacaDataStreamingClient :
        StreamingClientBase<AlpacaDataStreamingClientConfiguration>, 
        IAlpacaDataStreamingClient
    {
        private interface ISubscription
        {
            void OnUpdate();

            void OnReceived(
                JToken token);
        }

        private sealed class AlpacaDataSubscription<TApi, TJson>
            : IAlpacaDataSubscription<TApi>, ISubscription
            where TJson : class, TApi
            where TApi : IStreamBase
        {
            internal AlpacaDataSubscription(
                String stream) =>
                Stream = stream;

            public String Stream { get; }

            public IEnumerable<String> Streams
            {
                get { yield return Stream; }
            }

            public Boolean Subscribed { get; private set; }

            public event Action<TApi>? Received;

            public void OnReceived(
                JToken token) =>
                Received?.Invoke(token.ToObject<TJson>()
                                 ?? throw new RestClientErrorException());

            public void OnUpdate() => Subscribed = !Subscribed;
        }

        private sealed class Subscriptions
        {
            private const String WildcardSymbolString = "*";

            private readonly ConcurrentDictionary<String, ISubscription> _subscriptions =
                new ConcurrentDictionary<String, ISubscription>(StringComparer.Ordinal);

            public Subscriptions()
            {
                _subscriptions.GetOrAdd(MinuteAggChannel, _ =>
                    new AlpacaDataSubscription<IStreamAgg, JsonStreamAggAlpaca>(
                        getStreamName(MinuteAggChannel, WildcardSymbolString)));
            }

            public  IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
                String stream)
                where TJson : class, TApi
                where TApi : IStreamBase =>
                (IAlpacaDataSubscription<TApi>) _subscriptions.GetOrAdd(
                    stream, key => new AlpacaDataSubscription<TApi, TJson>(key));

            public void OnUpdate(String stream)
            {
                if (_subscriptions.TryGetValue(stream, out var subscription))
                {
                    subscription.OnUpdate();
                }
            }

            public Boolean OnReceived(
                String stream,
                JToken token)
            {
                var found = false;

                if (_subscriptions.TryGetValue(token["ev"]?.ToString() ?? String.Empty, out var subscription))
                {
                    subscription.OnReceived(token);
                    found = true;
                }

                if (_subscriptions.TryGetValue(stream, out subscription))
                {
                    subscription.OnReceived(token);
                    found = true;
                }

                return found;
            }
        }

        // Available Alpaca data streaming message types

        private const String TradesChannel = "T";

        private const String QuotesChannel = "Q";

        private const String MinuteAggChannel = "AM";

        private const String Listening = "listening";

        private const String Authorization = "authorization";

        private readonly IDictionary<String, Action<JToken>> _handlers;

        private readonly Subscriptions _subscriptions = new Subscriptions();

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataStreamingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaDataStreamingClient(
            AlpacaDataStreamingClientConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration))) =>
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { Listening, handleListeningUpdates },
                { Authorization, handleAuthorization }
            };

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamTrade> GetTradeSubscription(
            String symbol) => 
            _subscriptions.GetOrAdd<IStreamTrade, JsonStreamTradeAlpaca>(getStreamName(TradesChannel, symbol));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamQuote> GetQuoteSubscription(
            String symbol) =>
            _subscriptions.GetOrAdd<IStreamQuote, JsonStreamQuoteAlpaca>(getStreamName(QuotesChannel, symbol));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription() => 
            _subscriptions.GetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(MinuteAggChannel);

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            _subscriptions.GetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(getStreamName(MinuteAggChannel, symbol));

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
        protected override void OnMessageReceived(
            String message)
        {
            try
            {
                var token = JObject.Parse(message);

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
        
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private void handleListeningUpdates(
            JToken token)
        {
            var listeningUpdate = token.ToObject<JsonListeningUpdate>() ?? new JsonListeningUpdate();

            if (!String.IsNullOrEmpty(listeningUpdate.Error))
            {
                HandleError(new InvalidOperationException(listeningUpdate.Error));
            }

            foreach (var stream in listeningUpdate.Streams)
            {
                try
                {
                    _subscriptions.OnUpdate(stream);
                }
                catch (Exception exception)
                {
                    HandleError(exception);
                }
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
                return _subscriptions.OnReceived(stream, token);
            }
            catch (Exception exception)
            {
                HandleError(exception);
                return false;
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

        private static String getStreamName(
            String channelName,
            String symbol) =>
            $"{channelName}.{symbol}";
    }
}
