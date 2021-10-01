using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    internal abstract class DataStreamingClientBase<TConfiguration> :
        StreamingClientBase<TConfiguration>
        where TConfiguration : StreamingClientConfiguration
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

        private sealed class SubscriptionsDictionary
        {
            private readonly ConcurrentDictionary<String, ISubscription> _subscriptions = new (StringComparer.Ordinal);

            public  IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
                String stream)
                where TJson : class, TApi =>
                (IAlpacaDataSubscription<TApi>) _subscriptions.GetOrAdd(
                    stream, key => new AlpacaDataSubscription<TApi, TJson>(key));

            public void OnUpdate(
                ICollection<String> streams)
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

        private const String ErrorInfo = "error";

        private const String Subscription = "subscription";

        private const String ConnectionSuccess = "success";

        protected const String TradesChannel = "t";

        protected const String QuotesChannel = "q";

        protected const String DailyBarsChannel = "d";

        protected const String MinuteBarsChannel = "b";

        protected const String WildcardSymbolString = "*";

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Char[] _channelSeparator = { '.' };

        private readonly IDictionary<String, Action<JToken>> _handlers;

        private readonly SubscriptionsDictionary _subscriptions = new ();

        protected DataStreamingClientBase(
            TConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration))) =>
            _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
            {
                { MinuteBarsChannel, handleRealtimeDataUpdate },
                { DailyBarsChannel, handleRealtimeDataUpdate },
                { ConnectionSuccess, handleConnectionSuccess },
                { Subscription, handleSubscriptionUpdates },
                { TradesChannel, handleRealtimeDataUpdate },
                { QuotesChannel, handleRealtimeDataUpdate },
                { ErrorInfo, handleErrorMessages }
            };

        public ValueTask SubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default) =>
            subscribeAsync(subscription.EnsureNotNull(nameof(subscription)).Streams, cancellationToken);

        public ValueTask SubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default) =>
            subscribeAsync(subscriptions.SelectMany(_ => _.Streams), cancellationToken);

        public ValueTask UnsubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default) =>
            unsubscribeAsync(subscription.EnsureNotNull(nameof(subscription)).Streams, cancellationToken);

        public ValueTask UnsubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default) =>
            unsubscribeAsync(subscriptions.SelectMany(_ => _.Streams), cancellationToken);

        protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
            String channelName,
            String symbol)
            where TJson : class, TApi => 
            _subscriptions.GetOrAdd<TApi, TJson>(getStreamName(channelName, symbol));


        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected sealed override void OnMessageReceived(
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

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private async void handleConnectionSuccess(
            JToken token)
        {
            try
            {
                var connectionSuccess = token.ToObject<JsonConnectionSuccess>() ?? new JsonConnectionSuccess();

                // ReSharper disable once ConstantConditionalAccessQualifier
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (connectionSuccess.Status)
                {
                    case ConnectionStatus.Connected:
                        await SendAsJsonStringAsync(Configuration.SecurityId.GetAuthentication())
                            .ConfigureAwait(false);
                        break;

                    case ConnectionStatus.Authenticated:
                        OnConnected(AuthStatus.Authorized);
                        break;

                    default:
                        HandleError(new InvalidOperationException("Unknown connection status"));
                        break;
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
        private void handleSubscriptionUpdates(
            JToken token)
        {
            try
            {
                var subscriptionUpdate = token.ToObject<JsonSubscriptionUpdate>() ?? new JsonSubscriptionUpdate();

                var streams = new HashSet<String>(
                    getStreams(subscriptionUpdate.Trades.EmptyIfNull(), TradesChannel)
                        .Concat(getStreams(subscriptionUpdate.Quotes.EmptyIfNull(), QuotesChannel))
                        .Concat(getStreams(subscriptionUpdate.DailyBars.EmptyIfNull(), DailyBarsChannel))
                        .Concat(getStreams(subscriptionUpdate.MinuteBars.EmptyIfNull(), MinuteBarsChannel)),
                    StringComparer.Ordinal);

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

                if (String.Equals(channel, MinuteBarsChannel, StringComparison.Ordinal))
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
        private async void handleErrorMessages(
            JToken token)
        {
            try
            {
                var error = token.ToObject<JsonStreamError>() ?? new JsonStreamError();
                switch (error.Code)
                {
                    case 401: // Not authenticated
                        await SendAsJsonStringAsync(Configuration.SecurityId.GetAuthentication())
                            .ConfigureAwait(false);
                        break;

                    case 402: // Authentication failed
                    case 404: // Authentication timeout
                    case 406: // Connection limit exceeded
                        OnConnected(AuthStatus.Unauthorized);
                        break;

                    case 403: // Already authenticated
                        OnConnected(AuthStatus.Authorized);
                        break;

                    default:
                        HandleError(new RestClientErrorException(error));
                        break;
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        private ValueTask subscribeAsync(
            IEnumerable<String> streams,
            CancellationToken cancellationToken) =>
            sendSubscriptionRequestAsync(JsonAction.Subscribe, getLookup(streams), cancellationToken);

        private ValueTask unsubscribeAsync(
            IEnumerable<String> streams,
            CancellationToken cancellationToken) =>
            sendSubscriptionRequestAsync(JsonAction.Unsubscribe,getLookup(streams), cancellationToken);

        private ValueTask sendSubscriptionRequestAsync(
            JsonAction action,
            ILookup<String, String> streamsByChannels,
            CancellationToken cancellationToken) =>
            streamsByChannels.Count != 0
                ? SendAsJsonStringAsync(new JsonSubscriptionUpdate
                {
                    Action = action,
                    Trades = getSymbols(streamsByChannels, TradesChannel),
                    Quotes = getSymbols(streamsByChannels, QuotesChannel),
                    DailyBars = getSymbols(streamsByChannels, DailyBarsChannel),
                    MinuteBars = getSymbols(streamsByChannels, MinuteBarsChannel)
                }, cancellationToken)
                : new ValueTask();

        private static ILookup<String, String> getLookup(
            IEnumerable<String> streams) =>
            streams
                .Select(stream => stream.Split(
                    _channelSeparator, 2, StringSplitOptions.RemoveEmptyEntries))
                .Where(pair => pair.Length == 2)
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

        private static List<String>? getSymbols(
            ILookup<String, String> streamsByChannels,
            String stream) =>
            streamsByChannels[stream]
                .Where(_ => !String.IsNullOrEmpty(_))
                .ToList().NullIfEmpty();
    }
}