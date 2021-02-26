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
            void OnUpdate(
                Boolean isSubscribed);

            void OnReceived(
                JToken token);
        }

        private sealed class AlpacaDataSubscription<TApi, TJson>
            : IAlpacaDataSubscription<TApi>, ISubscription
            where TJson : class, TApi
            where TApi : IStreamBase
        {
            private readonly String _stream;

            internal AlpacaDataSubscription(
                String stream) =>
                _stream = stream;

            public IEnumerable<String> Streams
            {
                get { yield return _stream; }
            }

            public Boolean Subscribed { get; private set; }

            public event Action<TApi>? Received;

            public void OnReceived(
                JToken token) =>
                Received?.Invoke(token.ToObject<TJson>()
                                 ?? throw new RestClientErrorException());

            public void OnUpdate(
                Boolean isSubscribed) => Subscribed = isSubscribed;
        }

        private sealed class Subscriptions
        {
            private readonly ConcurrentDictionary<String, ISubscription> _subscriptions =
                new ConcurrentDictionary<String, ISubscription>(StringComparer.Ordinal);

            public  IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
                String stream)
                where TJson : class, TApi
                where TApi : IStreamBase =>
                (IAlpacaDataSubscription<TApi>) _subscriptions.GetOrAdd(
                    stream, key => new AlpacaDataSubscription<TApi, TJson>(key));

            public void OnUpdate(
                ISet<String> streams)
            {
                foreach (var kvp in _subscriptions)
                {
                    kvp.Value.OnUpdate(streams.Contains(kvp.Key));
                }
            }

            public void OnReceived(
                String stream,
                JToken token)
            {
                if (_subscriptions.TryGetValue(stream, out var subscription))
                {
                    subscription.OnReceived(token);
                }
            }
        }

        // Available Alpaca data streaming message types

        private const String TradesChannel = "t";

        private const String QuotesChannel = "q";

        private const String BarsChannel = "b";

        private const String ErrorInfo = "error";

        private const String Subscription = "subscription";

        private const String ConnectionSuccess = "success";

        private const String WildcardSymbolString = "*";

        private static readonly Char[] ChannelSeparator = { '.' };

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
                { ConnectionSuccess, handleConnectionSuccess },
                { Subscription, handleSubscriptionUpdates },
                { TradesChannel, handleRealtimeDataUpdate },
                { QuotesChannel, handleRealtimeDataUpdate },
                { BarsChannel, handleRealtimeDataUpdate },
                { ErrorInfo, handleErrorMessages }
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
            _subscriptions.GetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(getStreamName(BarsChannel, WildcardSymbolString));

        /// <inheritdoc />
        public IAlpacaDataSubscription<IStreamAgg> GetMinuteAggSubscription(
            String symbol) =>
            _subscriptions.GetOrAdd<IStreamAgg, JsonStreamAggAlpaca>(getStreamName(BarsChannel, symbol));

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
                    var messageType = token["T"];
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

        private void handleConnectionSuccess(
            JToken token)
        {
            var connectionSuccess = token.ToObject<JsonConnectionSuccess>() ?? new JsonConnectionSuccess();

            // ReSharper disable once ConstantConditionalAccessQualifier
            switch (connectionSuccess.Status)
            {
                case ConnectionStatus.Connected:
                    SendAsJsonString(Configuration.SecurityId.GetAuthentication());
                    break;

                case ConnectionStatus.Authenticated:
                    OnConnected(AuthStatus.Authorized);
                    break;

                default:
                    HandleError(new InvalidOperationException("Unknown connection status"));
                    break;
            }
        }
        
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private void handleSubscriptionUpdates(
            JToken token)
        {
            var subscriptionUpdate = token.ToObject<JsonSubscriptionUpdate>() ?? new JsonSubscriptionUpdate();

            var streams = new HashSet<String>(
                getStreams(subscriptionUpdate.Trades, TradesChannel)
                    .Concat(getStreams(subscriptionUpdate.Quotes, QuotesChannel))
                    .Concat(getStreams(subscriptionUpdate.Bars, BarsChannel)),
                StringComparer.Ordinal);

            try
            {
                _subscriptions.OnUpdate(streams);
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private void handleRealtimeDataUpdate(
            JToken token)
        {
            try
            {
                var channel = token["T"]?.ToString() ?? String.Empty;
                var symbol = token["S"]?.ToString() ?? String.Empty;

                _subscriptions.OnReceived(getStreamName(channel, symbol), token);

                if (String.Equals(channel, BarsChannel, StringComparison.Ordinal))
                {
                    _subscriptions.OnReceived(getStreamName(channel, WildcardSymbolString), token);
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
        private void handleErrorMessages(
            JToken token)
        {
            try
            {
                var error = token.ToObject<JsonStreamError>() ?? new JsonStreamError();
                switch (error.Code)
                {
                    case 401: // Not authenticated
                    case 402: // Authentication failed
                    case 404: // Authentication timeout
                    case 406: // Connection limit exceeded
                        OnConnected(AuthStatus.Unauthorized);
                        break;

                    case 403: // Already authenticated
                        break;
                }
                HandleError(new RestClientErrorException(error));
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private void subscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(getLookup(streams), JsonAction.PolygonSubscribe);

        private void unsubscribe(
            IEnumerable<String> streams) =>
            sendSubscriptionRequest(getLookup(streams), JsonAction.PolygonUnsubscribe);

        private void sendSubscriptionRequest(
            ILookup<String, String> streamsByChannels,
            JsonAction action) =>
            SendAsJsonString(new JsonSubscriptionUpdate
            {
                Action = action,
                Trades = streamsByChannels[TradesChannel].ToList(),
                Quotes = streamsByChannels[QuotesChannel].ToList(),
                Bars = streamsByChannels[BarsChannel].ToList()
            });

        private static ILookup<String, String> getLookup(
            IEnumerable<String> streams) =>
            streams
                .Select(stream => stream.Split(
                    ChannelSeparator, 2, StringSplitOptions.RemoveEmptyEntries))
                .ToLookup(
                    pair => pair[0], 
                    pair => pair[1], 
                    StringComparer.Ordinal);

        private static IEnumerable<String> getStreams(
            IEnumerable<String> symbols,
            String channelName) =>
            symbols.Select(_ => getStreamName(channelName, _));

        private static String getStreamName(
            String channelName,
            String symbol) =>
            $"{channelName}.{symbol}";
    }
}
