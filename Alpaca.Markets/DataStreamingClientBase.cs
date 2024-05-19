using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets;

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

    private sealed class AlpacaExplicitDataSubscription<TApi, TJson>
        : IAlpacaDataSubscription<TApi>, ISubscription
        where TJson : class, TApi
    {
        private readonly String _stream;

        private Boolean _subscribed;

        internal AlpacaExplicitDataSubscription(
            String stream) =>
            _stream = stream;

        public IEnumerable<String> Streams
        {
            get { yield return _stream; }
        }

        public Boolean Subscribed
        {
            get => _subscribed;
            private set
            {
                if (_subscribed == value)
                {
                    return;
                }

                _subscribed = value;
                OnSubscribedChanged?.Invoke();
            }
        }

        public event Action<TApi>? Received;

        public event Action? OnSubscribedChanged;

        public void OnReceived(
            JToken token) =>
            Received?.Invoke(token.ToObject<TJson>() ?? throw new RestClientErrorException());

        public void OnUpdate(
            Boolean isSubscribed) => Subscribed = isSubscribed;
    }

    private sealed class AlpacaImplicitDataSubscription<TApi, TJson>
        : IAlpacaDataSubscription<TApi>, ISubscription
        where TJson : class, TApi
    {
        private readonly IAlpacaDataSubscription _parent;

        private readonly String _stream;

        internal AlpacaImplicitDataSubscription(
            String stream,
            IAlpacaDataSubscription parent)
        {
            _stream = stream;
            _parent = parent;
        }

        public IEnumerable<String> Streams
        {
            get { yield return _stream; }
        }

        public Boolean Subscribed => _parent.Subscribed;

        public event Action<TApi>? Received;

        public event Action? OnSubscribedChanged
        {
            add => _parent.OnSubscribedChanged += value;
            remove => _parent.OnSubscribedChanged -= value;
        }

        public void OnReceived(
            JToken token) =>
            Received?.Invoke(token.ToObject<TJson>() ?? throw new RestClientErrorException());

        public void OnUpdate(
            Boolean isSubscribed) =>
            Trace.WriteLine($"Update received for the channel '{_stream}' - not expected.");
    }

    private sealed class SubscriptionsDictionary
    {
        private readonly ConcurrentDictionary<String, ISubscription> _subscriptions = new(StringComparer.Ordinal);

        public IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
            String stream)
            where TJson : class, TApi =>
            (IAlpacaDataSubscription<TApi>)_subscriptions.GetOrAdd(
                stream, key => new AlpacaExplicitDataSubscription<TApi, TJson>(key));

        public IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
            String stream,
            IAlpacaDataSubscription parent)
            where TJson : class, TApi
        {
            return (IAlpacaDataSubscription<TApi>)_subscriptions.GetOrAdd(
                stream, key => new AlpacaImplicitDataSubscription<TApi, TJson>(key, parent));
        }

        public void OnUpdate(
            ICollection<String> streams)
        {
            foreach (var (stream, subscription) in _subscriptions)
            {
                subscription.OnUpdate(streams.Contains(stream));
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

    protected const String NewsChannel = "n";

    private const String TradesChannel = "t";

    protected const String QuotesChannel = "q";

    private const String DailyBarsChannel = "d";

    protected const String StatusesChannel = "s";

    private const String MinuteBarsChannel = "b";

    private const String UpdatedBarsChannel = "u";

    protected const String OrderBooksChannel = "o";

    protected const String LimitUpDownChannel = "l";

    protected const String CorrectionsChannel = "c";

    protected const String CancellationsChannel = "x";

    protected const String WildcardSymbolString = "*";

    private const Int32 SubscriptionChunkSize = 100;

    // ReSharper disable once StaticMemberInGenericType
    private static readonly Char[] _channelSeparator = [ '.' ];

    // ReSharper disable once StaticMemberInGenericType
    private static readonly SortedSet<String> _implicitChannels = new(StringComparer.Ordinal)
    {
        CancellationsChannel,
        CorrectionsChannel
    };

    private readonly IDictionary<String, Action<JToken>> _handlers;

    private readonly SubscriptionsDictionary _subscriptions = new();

    protected DataStreamingClientBase(
        TConfiguration configuration)
        : base(configuration.EnsureNotNull()) =>
        _handlers = new Dictionary<String, Action<JToken>>(StringComparer.Ordinal)
        {
                { CancellationsChannel, handleRealtimeDataUpdate },
                { CorrectionsChannel, handleRealtimeDataUpdate },
                { LimitUpDownChannel, handleRealtimeDataUpdate },
                { UpdatedBarsChannel, handleRealtimeDataUpdate },
                { MinuteBarsChannel, handleRealtimeDataUpdate },
                { OrderBooksChannel, handleRealtimeDataUpdate },
                { DailyBarsChannel, handleRealtimeDataUpdate },
                { ConnectionSuccess, handleConnectionSuccess },
                { StatusesChannel, handleRealtimeDataUpdate },
                { Subscription, handleSubscriptionUpdates },
                { TradesChannel, handleRealtimeDataUpdate },
                { QuotesChannel, handleRealtimeDataUpdate },
                { NewsChannel, handleRealtimeNewsUpdate },
                { ErrorInfo, handleErrorMessages }
        };

    public IAlpacaDataSubscription<ITrade> GetTradeSubscription() =>
        GetSubscription<ITrade, JsonRealTimeTrade>(TradesChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<ITrade> GetTradeSubscription(
        String symbol) =>
        GetSubscription<ITrade, JsonRealTimeTrade>(TradesChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription() =>
        GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, WildcardSymbolString);

    public IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(MinuteBarsChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(DailyBarsChannel, symbol.EnsureNotNull());

    public IAlpacaDataSubscription<IBar> GetUpdatedBarSubscription(
        String symbol) =>
        GetSubscription<IBar, JsonRealTimeBar>(UpdatedBarsChannel, symbol.EnsureNotNull());

    public ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription) =>
        SubscribeAsync(subscription, CancellationToken.None);

    public ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken) =>
        subscribeAsync(subscription.EnsureNotNull().Streams, cancellationToken);

    public ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions) =>
        SubscribeAsync(subscriptions, CancellationToken.None);

    public ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken) =>
        subscribeAsync(subscriptions.SelectMany(subscription => subscription.Streams), cancellationToken);

    public ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription) =>
        UnsubscribeAsync(subscription, CancellationToken.None);

    public ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken) =>
        unsubscribeAsync(subscription.EnsureNotNull().Streams, cancellationToken);

    public ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions) =>
        UnsubscribeAsync(subscriptions, CancellationToken.None);

    public ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken) =>
        unsubscribeAsync(subscriptions.SelectMany(subscription => subscription.Streams), cancellationToken);

    protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
        String channelName,
        String symbol)
        where TJson : class, TApi =>
        _subscriptions.GetOrAdd<TApi, TJson>(getStreamName(channelName, symbol));

    protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
        String channelName,
        String symbol,
        IAlpacaDataSubscription parent)
        where TJson : class, TApi =>
        _subscriptions.GetOrAdd<TApi, TJson>(getStreamName(channelName, symbol), parent);

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
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
                    HandleWarning("Incoming message missing message type.");
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

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
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
                    HandleWarning($"Unknown connection status `{connectionSuccess.Status}` found.");
                    break;
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
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
                    .Concat(getStreams(subscriptionUpdate.News.EmptyIfNull(), NewsChannel))
                    .Concat(getStreams(subscriptionUpdate.Quotes.EmptyIfNull(), QuotesChannel))
                    .Concat(getStreams(subscriptionUpdate.Statuses.EmptyIfNull(), StatusesChannel))
                    .Concat(getStreams(subscriptionUpdate.Lulds.EmptyIfNull(), LimitUpDownChannel))
                    .Concat(getStreams(subscriptionUpdate.DailyBars.EmptyIfNull(), DailyBarsChannel))
                    .Concat(getStreams(subscriptionUpdate.OrderBooks.EmptyIfNull(), OrderBooksChannel))
                    .Concat(getStreams(subscriptionUpdate.MinuteBars.EmptyIfNull(), MinuteBarsChannel))
                    .Concat(getStreams(subscriptionUpdate.UpdatedBars.EmptyIfNull(), UpdatedBarsChannel)),
                StringComparer.Ordinal);

            _subscriptions.OnUpdate(streams);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
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
            _subscriptions.OnReceived(getStreamName(channel, WildcardSymbolString), token);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private void handleRealtimeNewsUpdate(
        JToken token)
    {
        try
        {
            var channel = token["T"]?.ToString() ?? String.Empty;
            var symbols = token["symbols"]?.Values<String>() ?? [];

            foreach (var symbol in symbols.Where(value => !String.IsNullOrEmpty(value)))
            {
                _subscriptions.OnReceived(getStreamName(channel, symbol!), token);
            }

            _subscriptions.OnReceived(getStreamName(channel, WildcardSymbolString), token);
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

#pragma warning disable IDE0079
    [SuppressMessage(
#pragma warning restore IDE0079
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private async void handleErrorMessages(
        JToken token)
    {
        const Int32 connectionLimitExceeded = 406;
        const Int32 authenticationTimeout = 404;
        const Int32 alreadyAuthenticated = 403;
        const Int32 authenticationFailed = 402;
        const Int32 notAuthenticated = 401;

        try
        {
            var error = token.ToObject<JsonStreamError>() ?? new JsonStreamError();
            switch (error.Code)
            {
                case notAuthenticated:
                    await SendAsJsonStringAsync(Configuration.SecurityId.GetAuthentication())
                        .ConfigureAwait(false);
                    break;

                case connectionLimitExceeded:
                    OnConnected(AuthStatus.TooManyConnections);
                    break;

                case authenticationTimeout:
                case authenticationFailed:
                    OnConnected(AuthStatus.Unauthorized);
                    break;

                case alreadyAuthenticated:
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
        sendSubscriptionRequestsAsync(JsonAction.Subscribe, streams, cancellationToken);

    private ValueTask unsubscribeAsync(
        IEnumerable<String> streams,
        CancellationToken cancellationToken) =>
        sendSubscriptionRequestsAsync(JsonAction.Unsubscribe, streams, cancellationToken);

    private async ValueTask sendSubscriptionRequestsAsync(
        JsonAction action,
        IEnumerable<String> streams,
        CancellationToken cancellationToken)
    {
        foreach (var chunk in streams.Chunk(SubscriptionChunkSize))
        {
            await sendSubscriptionRequestAsync(
                action, getLookup(chunk), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private ValueTask sendSubscriptionRequestAsync(
        JsonAction action,
        ILookup<String, String> streamsByChannels,
        CancellationToken cancellationToken) =>
        streamsByChannels.Count != 0
            ? SendAsJsonStringAsync(new JsonSubscriptionUpdate
            {
                Action = action,
                News = getSymbols(streamsByChannels, NewsChannel),
                Trades = getSymbols(streamsByChannels, TradesChannel),
                Quotes = getSymbols(streamsByChannels, QuotesChannel),
                Statuses = getSymbols(streamsByChannels, StatusesChannel),
                Lulds = getSymbols(streamsByChannels, LimitUpDownChannel),
                DailyBars = getSymbols(streamsByChannels, DailyBarsChannel),
                MinuteBars = getSymbols(streamsByChannels, MinuteBarsChannel),
                OrderBooks = getSymbols(streamsByChannels, OrderBooksChannel),
                UpdatedBars = getSymbols(streamsByChannels, UpdatedBarsChannel)
            }, cancellationToken)
            : new ValueTask();

    private static ILookup<String, String> getLookup(
        IEnumerable<String> streams) =>
        streams
            .Select(stream => stream.Split(
                _channelSeparator, 2, StringSplitOptions.RemoveEmptyEntries))
            .Where(pair => pair.Length == 2)
            .Where(pair => !_implicitChannels.Contains(pair[0]))
            .ToLookup(
                pair => pair[0],
                pair => pair[1],
                StringComparer.Ordinal);

    private static IEnumerable<String> getStreams(
        IEnumerable<String> symbols,
        String channelName) =>
        symbols.Select(symbol => getStreamName(channelName, symbol));

    private static String getStreamName(
        String channelName,
        String symbol) =>
        $"{channelName}.{symbol}";

    private static List<String>? getSymbols(
        ILookup<String, String> streamsByChannels,
        String stream) =>
        streamsByChannels[stream]
            .Where(symbol => !String.IsNullOrEmpty(symbol))
            .ToList().NullIfEmpty();
}
